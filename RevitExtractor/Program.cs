using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using System;
using System.Collections.Generic;

namespace RevitExtractor
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: RevitExtractor <rvt-file> <connection-string>");
                return 1;
            }

            string modelPath = args[0];
            string conn = args[1];

            // Start Revit in automation mode. Requires Revit to be installed locally.
            ControlledApplication ctrlApp = new ControlledApplication();
            Application app = ctrlApp.Create.NewApplication();
            ModelPath mp = ModelPathUtils.ConvertUserVisiblePathToModelPath(modelPath);
            OpenOptions opts = new OpenOptions();
            Document doc = app.OpenDocumentFile(mp, opts);

            var db = new PostgresDb(conn);
            DateTime lastSaved = System.IO.File.GetLastWriteTime(modelPath);

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
                string typeName = string.Empty;
                if (typeMap.TryGetValue(element.GetTypeId(), out string tn))
                    typeName = tn;

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
                        param.IsShared || param.IsReadOnly, new[] { element.Category?.Name ?? string.Empty }, lastSaved);
                }
            }

            doc.Close(false);
            app.Dispose();
            ctrlApp.Dispose();
            return 0;
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
