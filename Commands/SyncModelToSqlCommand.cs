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

        string conn = DbConfigHelper.GetConnectionString(input);
        if (string.IsNullOrEmpty(conn))
        {
            response["status"] = "error";
            response["message"] = "No connection string found. " + DbConfigHelper.GetHelpMessage();
            return response;
        }

        // Run connection test before instantiating PostgresDb
        try
        {
            using (var testConn = new Npgsql.NpgsqlConnection(conn))
            {
                testConn.Open(); // This will throw if Npgsql or dependencies are broken
            }
        }
        catch (Exception ex)
        {
            System.IO.File.WriteAllText("C:\\Temp\\pg-debug.txt", ex.ToString());
            response["status"] = "error";
            response["message"] = "Connection test failed. See pg-debug.txt for details.";
            return response;
        }

        // Proceed as usual if the test passed
        var db = new PostgresDb(conn);
        DateTime now = DateTime.UtcNow;

        // gather element types once and store in DB
        var typeCollector = new FilteredElementCollector(doc).WhereElementIsElementType();
        var typeMap = new Dictionary<ElementId, string>();
        foreach (ElementType type in typeCollector)
        {
            db.UpsertElementType(type.Id.IntegerValue, ParseGuid(type.UniqueId), type.FamilyName, type.Name, type.Category?.Name ?? string.Empty, doc.PathName, now);
            if (!typeMap.ContainsKey(type.Id))
                typeMap[type.Id] = type.Name;
        }

        var collector = new FilteredElementCollector(doc).WhereElementIsNotElementType();
        int count = 0;
        foreach (var element in collector)
        {
            string typeName = string.Empty;
            if (typeMap.TryGetValue(element.GetTypeId(), out string tname))
                typeName = tname;

            string levelName = string.Empty;
            if (element.LevelId != ElementId.InvalidElementId)
            {
                var lvl = doc.GetElement(element.LevelId);
                if (lvl != null) levelName = lvl.Name;
            }
            db.UpsertElement(element.Id.IntegerValue, ParseGuid(element.UniqueId), element.Name, element.Category?.Name ?? string.Empty, typeName, levelName, doc.PathName, now);
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