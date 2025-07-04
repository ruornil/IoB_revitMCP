
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

namespace RevitExtractor
{
    /// <summary>
    /// Revit external command that exports element types, instances and their
    /// parameters to a PostgreSQL database.
    /// </summary>
    
    [Transaction(TransactionMode.Manual)]
    public class ExportModelToPostgresCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument?.Document;
            if (doc == null)
            {
                message = "No active document.";
                return Result.Failed;
            }

            string dllDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string connPath = System.IO.Path.Combine(dllDir, "revit-conn.txt");

            if (!System.IO.File.Exists(connPath))
            {
                message = $"Connection file not found: {connPath}";
                return Result.Failed;
            }

            string conn = System.IO.File.ReadAllText(connPath).Trim();
            var db = new PostgresDb(conn);

            string modelPath = doc.PathName;
            DateTime lastSaved = System.IO.File.GetLastWriteTime(modelPath);

            try
            {
                // Extract element types
                var typeCollector = new FilteredElementCollector(doc).WhereElementIsElementType();
                var typeMap = new Dictionary<ElementId, string>();
                foreach (ElementType type in typeCollector)
                {
                    db.UpsertElementType(type.Id.IntegerValue, ParseGuid(type.UniqueId), type.FamilyName,
                        type.Name, type.Category?.Name ?? string.Empty, modelPath, lastSaved);
                    if (!typeMap.ContainsKey(type.Id))
                        typeMap[type.Id] = type.Name;
                }

                // Extract elements and parameters
                var collector = new FilteredElementCollector(doc).WhereElementIsNotElementType();
                foreach (var element in collector)
                {
                    string typeName = typeMap.TryGetValue(element.GetTypeId(), out var tn) ? tn : string.Empty;

                    string levelName = string.Empty;
                    if (element.LevelId != ElementId.InvalidElementId)
                    {
                        var lvl = doc.GetElement(element.LevelId) as Level;
                        if (lvl != null) levelName = lvl.Name;
                    }

                    db.UpsertElement(element.Id.IntegerValue, ParseGuid(element.UniqueId), element.Name,
                        element.Category?.Name ?? string.Empty, typeName, levelName, modelPath, lastSaved);

                    foreach (Parameter param in element.Parameters)
                    {
                        string val = ParamToString(param);
                        db.UpsertParameter(element.Id.IntegerValue, param.Definition.Name, val,
                            param.IsShared || param.IsReadOnly,
                            new[] { element.Category?.Name ?? string.Empty }, lastSaved);
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }

            return Result.Succeeded;
        }

        private static string ParamToString(Parameter p)
        {
            switch (p.StorageType)
            {
                case StorageType.String:
                    return p.AsString();
                case StorageType.Integer:
                    return p.AsInteger().ToString();
                case StorageType.Double:
                    return p.AsDouble().ToString();
                case StorageType.ElementId:
                    return p.AsElementId().IntegerValue.ToString();
                default:
                    return string.Empty;
            }
        }

        private static Guid ParseGuid(string uid)
        {
            if (string.IsNullOrEmpty(uid)) return Guid.Empty;
            if (uid.Length >= 36 && Guid.TryParse(uid.Substring(0, 36), out var g))
                return g;
            return Guid.Empty;
        }
    }
}
