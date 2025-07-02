// ListElementsCommand.cs - example of a typed commandset handler
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;

public class ListElementsCommand : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var doc = app.ActiveUIDocument.Document;
        var response = new Dictionary<string, object>();
        string categoryName = input.ContainsKey("category") ? input["category"] : "Walls";

        string conn = DbConfigHelper.GetConnectionString(input);
        PostgresDb db = null;
        if (!string.IsNullOrEmpty(conn))
            db = new PostgresDb(conn);

        DateTime lastSaved = System.IO.File.GetLastWriteTime(doc.PathName);

        var collector = new FilteredElementCollector(doc)
            .OfCategory(CategoryUtils.ParseBuiltInCategory(categoryName))
            .WhereElementIsNotElementType();

        var elements = new List<Dictionary<string, object>>();
        NpgsqlConnection openConn = null;
        NpgsqlTransaction tx = null;
        if (db != null)
        {
            openConn = new NpgsqlConnection(conn);
            openConn.Open();
            tx = openConn.BeginTransaction();
        }

        foreach (var e in collector)
        {
            var item = new Dictionary<string, object>();
            item["id"] = e.Id.IntegerValue;
            item["name"] = e.Name;
            item["doc_id"] = doc.PathName;
            elements.Add(item);

            if (db != null)
            {
                string typeName = string.Empty;
                var et = doc.GetElement(e.GetTypeId()) as ElementType;
                if (et != null) typeName = et.Name;

                string levelName = string.Empty;
                if (e is Element el && el.LevelId != ElementId.InvalidElementId)
                {
                    var lvl = doc.GetElement(el.LevelId);
                    if (lvl != null) levelName = lvl.Name;
                }

                db.UpsertElement(openConn, e.Id.IntegerValue, ParseGuid(e.UniqueId), e.Name,
                    e.Category?.Name ?? string.Empty, typeName, levelName, doc.PathName, lastSaved, tx);
            }
        }

        if (db != null)
        {
            db.UpsertModelInfo(openConn, doc.PathName, doc.Title, ParseGuid(doc.ProjectInformation.UniqueId), lastSaved, null, null, tx);
            tx.Commit();
            openConn.Close();
        }

        response["status"] = "success";
        response["elements"] = elements;
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