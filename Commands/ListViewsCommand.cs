using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

public class ListViewsCommand : ICommand
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

        var result = new List<Dictionary<string, object>>();
        try
        {
            string conn = ConfigurationManager.ConnectionStrings["revit"]?.ConnectionString;
            PostgresDb db = null;
            if (!string.IsNullOrEmpty(conn))
                db = new PostgresDb(conn);

            var views = new FilteredElementCollector(doc)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v => !v.IsTemplate);

            foreach (var view in views)
            {
                var item = new Dictionary<string, object>();
                item["id"] = view.Id.IntegerValue;
                item["guid"] = view.UniqueId;
                item["name"] = view.Name;
                item["view_type"] = view.ViewType.ToString();
                item["scale"] = view.Scale;
                item["discipline"] = view.Discipline.ToString();
                item["detail_level"] = view.DetailLevel.ToString();
                var vp = new FilteredElementCollector(doc).OfClass(typeof(Viewport)).Cast<Viewport>().FirstOrDefault(v => v.ViewId == view.Id);
                item["associated_sheet_id"] = vp != null ? (int?)vp.SheetId.IntegerValue : null;
                item["doc_id"] = doc.PathName;
                result.Add(item);

                if (db != null)
                {
                    int? sheetId = vp != null ? (int?)vp.SheetId.IntegerValue : null;
                    db.UpsertView(view.Id.IntegerValue, Guid.Empty, view.Name, view.ViewType.ToString(), view.Scale, view.Discipline.ToString(), view.DetailLevel.ToString(), sheetId, doc.PathName);
                }
            }
            response["status"] = "success";
            response["views"] = result;
        }
        catch (Exception ex)
        {
            response["status"] = "error";
            response["message"] = ex.Message;
        }
        return response;
    }
}
