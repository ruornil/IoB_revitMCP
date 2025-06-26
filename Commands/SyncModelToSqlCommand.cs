using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Configuration;

public class SyncModelToSqlCommand : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var response = new Dictionary<string, object>();
        var doc = app.ActiveUIDocument?.Document;
        if (doc == null)
        {
            response["status"] = "error";
            response["message"] = "No active document.";
            return response;
        }

        string conn = ConfigurationManager.ConnectionStrings["revit"]?.ConnectionString;
        if (string.IsNullOrEmpty(conn))
        {
            response["status"] = "error";
            response["message"] = "No connection string.";
            return response;
        }

        var db = new PostgresDb(conn);
        var collector = new FilteredElementCollector(doc).WhereElementIsNotElementType();
        DateTime now = DateTime.UtcNow;
        int count = 0;
        foreach (var element in collector)
        {
            string typeName = string.Empty;
            ElementType type = doc.GetElement(element.GetTypeId()) as ElementType;
            if (type != null)
                typeName = type.Name;
            string levelName = string.Empty;
            if (element.LevelId != ElementId.InvalidElementId)
            {
                var lvl = doc.GetElement(element.LevelId);
                if (lvl != null) levelName = lvl.Name;
            }
            db.UpsertElement(element.Id.IntegerValue, ParseGuid(element.UniqueId), element.Name, element.Category?.Name ?? string.Empty, typeName, levelName, doc.PathName, now);

            if (type != null)
            {
                db.UpsertElementType(type.Id.IntegerValue, ParseGuid(type.UniqueId), type.FamilyName, type.Name, type.Category?.Name ?? string.Empty, doc.PathName, now);
            }
            count++;
        }
        response["status"] = "success";
        response["updated"] = count;
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
