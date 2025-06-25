using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

public class ListSchedulesCommand : ICommand
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

        var scheds = new List<Dictionary<string, object>>();
        try
        {
            string conn = ConfigurationManager.ConnectionStrings["revit"]?.ConnectionString;
            PostgresDb db = null;
            if (!string.IsNullOrEmpty(conn))
                db = new PostgresDb(conn);

            var col = new FilteredElementCollector(doc).OfClass(typeof(ViewSchedule)).Cast<ViewSchedule>();
            foreach (var sch in col)
            {
                var item = new Dictionary<string, object>();
                item["id"] = sch.Id.IntegerValue;
                item["guid"] = sch.UniqueId;
                item["name"] = sch.Name;
                item["category"] = sch.Definition?.CategoryId != null ? doc.GetElement(sch.Definition.CategoryId)?.Name : string.Empty;
                item["doc_id"] = doc.PathName;
                scheds.Add(item);

                if (db != null)
                {
                    db.UpsertSchedule(sch.Id.IntegerValue, Guid.Empty, sch.Name, item["category"].ToString(), doc.PathName);
                }
            }
            response["status"] = "success";
            response["schedules"] = scheds;
        }
        catch (Exception ex)
        {
            response["status"] = "error";
            response["message"] = ex.Message;
        }
        return response;
    }
}
