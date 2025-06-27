using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
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
            string conn = DbConfigHelper.GetConnectionString(input);
            if (string.IsNullOrEmpty(conn))
            {
                response["status"] = "error";
                response["message"] = "No connection string found. " + DbConfigHelper.GetHelpMessage();
                return response;
            }

            PostgresDb db = new PostgresDb(conn);
            DateTime lastSaved = System.IO.File.GetLastWriteTime(doc.PathName);
            if (db.GetModelLastSaved(doc.PathName) == lastSaved)
            {
                response["status"] = "up_to_date";
                return response;
            }

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

                // Discipline is not available in Revit 2023, so use a placeholder
                string discipline = "Unknown";
                item["discipline"] = discipline;

                item["detail_level"] = view.DetailLevel.ToString();
                var vp = new FilteredElementCollector(doc).OfClass(typeof(Viewport)).Cast<Viewport>().FirstOrDefault(v => v.ViewId == view.Id);
                item["associated_sheet_id"] = vp != null ? (int?)vp.SheetId.IntegerValue : null;
                item["doc_id"] = doc.PathName;
                result.Add(item);

                int? sheetId = vp != null ? (int?)vp.SheetId.IntegerValue : null;
                db.UpsertView(view.Id.IntegerValue, Guid.Empty, view.Name, view.ViewType.ToString(), view.Scale, discipline, view.DetailLevel.ToString(), sheetId, doc.PathName, lastSaved);
            }
            db.UpsertModelInfo(doc.PathName, doc.Title, ParseGuid(doc.ProjectInformation.UniqueId), lastSaved);
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
