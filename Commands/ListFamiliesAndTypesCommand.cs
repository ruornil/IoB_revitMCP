// GetFamiliesAndTypesCommand.cs - lists all families and their types in the model
// Extended to include family GUID and document id
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Npgsql;

public class ListFamiliesAndTypesCommand : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var doc = app.ActiveUIDocument.Document;
        var response = new Dictionary<string, object>();
        var result = new List<Dictionary<string, object>>();

        string conn = DbConfigHelper.GetConnectionString(input);
        PostgresDb db = null;
        if (!string.IsNullOrEmpty(conn))
            db = new PostgresDb(conn);

        DateTime lastSaved = System.IO.File.GetLastWriteTime(doc.PathName);
        if (db != null && db.GetModelLastSaved(doc.PathName) == lastSaved)
        {
            response["status"] = "up_to_date";
            return response;
        }

        try
        {
            Type filterType = typeof(ElementType);

            if (input.ContainsKey("class_name"))
            {
                string className = input["class_name"];
                var revitApiAssembly = typeof(FamilyInstance).Assembly;
                var resolved = revitApiAssembly.GetTypes().FirstOrDefault(t => t.Name == className);

                if (resolved != null && typeof(ElementType).IsAssignableFrom(resolved))
                {
                    filterType = resolved;
                }
                else
                {
                    response["status"] = "error";
                    response["message"] = $"'{className}' is not a valid Revit ElementType class.";
                    return response;
                }
            }

            var types = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .OfClass(filterType)
                .Cast<ElementType>();

            if (db != null)
            {
                using var openConn = new NpgsqlConnection(conn);
                openConn.Open();
                using var tx = openConn.BeginTransaction();

                foreach (var type in types)
                {
                    if (type.FamilyName != null && type.Name != null)
                    {
                        var item = new Dictionary<string, object>();
                        item["family"] = type.FamilyName;
                        item["type"] = type.Name;
                        item["id"] = type.Id.IntegerValue.ToString();
                        item["category"] = type.Category?.Name ?? string.Empty;
                        item["guid"] = type.UniqueId;
                        item["doc_id"] = doc.PathName;
                        result.Add(item);

                        db.UpsertFamily(openConn, type.FamilyName, type.Name, type.Category?.Name ?? string.Empty, type.UniqueId, doc.PathName, lastSaved, tx);
                    }
                }

                db.UpsertModelInfo(openConn, doc.PathName, doc.Title, ParseGuid(doc.ProjectInformation.UniqueId), lastSaved, null, null, tx);
                tx.Commit();
            }
            else
            {
                foreach (var type in types)
                {
                    if (type.FamilyName != null && type.Name != null)
                    {
                        var item = new Dictionary<string, object>();
                        item["family"] = type.FamilyName;
                        item["type"] = type.Name;
                        item["id"] = type.Id.IntegerValue.ToString();
                        item["category"] = type.Category?.Name ?? string.Empty;
                        item["guid"] = type.UniqueId;
                        item["doc_id"] = doc.PathName;
                        result.Add(item);
                    }
                }
            }

            response["status"] = "success";
            response["types"] = result;
        }
        catch (Exception ex)
        {
            response["status"] = "error";
            response["message"] = ex.Message;
        }

        return response;
    }

    private static Guid ParseGuid(string uid)
    {
        if (string.IsNullOrEmpty(uid)) return Guid.Empty;
        if (uid.Length >= 36)
        {
            Guid g;
            if (Guid.TryParse(uid.Substring(0, 36), out g)) return g;
        }
        return Guid.Empty;
    }
}
