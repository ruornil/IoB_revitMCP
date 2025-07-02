using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;

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
            PostgresDb db = null;
            if (!string.IsNullOrEmpty(conn))
                db = new PostgresDb(conn);

            DateTime lastSaved = System.IO.File.GetLastWriteTime(doc.PathName);

            if (ModelCache.TryGet(doc.PathName + "/views", lastSaved, out List<Dictionary<string, object>> cached))
            {
                response["status"] = "success";
                response["views"] = cached;
                return response;
            }

            if (db != null && db.GetModelLastSaved(doc.PathName) == lastSaved)
            {
                response["status"] = "up_to_date";
                return response;
            }

            var viewports = new FilteredElementCollector(doc)
                .OfClass(typeof(Viewport))
                .Cast<Viewport>()
                .GroupBy(v => v.ViewId.IntegerValue)
                .ToDictionary(g => g.Key, g => (int?)g.First().SheetId.IntegerValue);

            var views = new FilteredElementCollector(doc)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v => !v.IsTemplate);

            if (db != null)
            {
                using var openConn = new NpgsqlConnection(conn);
                openConn.Open();
                using var tx = openConn.BeginTransaction();

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
                    viewports.TryGetValue(view.Id.IntegerValue, out int? sheetId);
                    item["associated_sheet_id"] = sheetId;
                    item["doc_id"] = doc.PathName;
                    result.Add(item);

                    db.UpsertView(openConn, view.Id.IntegerValue, Guid.Empty, view.Name, view.ViewType.ToString(), view.Scale, discipline, view.DetailLevel.ToString(), sheetId, doc.PathName, lastSaved, tx);
                }

                db.UpsertModelInfo(openConn, doc.PathName, doc.Title, ParseGuid(doc.ProjectInformation.UniqueId), lastSaved, null, null, tx);
                tx.Commit();
            }
            else
            {
                foreach (var view in views)
                {
                    var item = new Dictionary<string, object>();
                    item["id"] = view.Id.IntegerValue;
                    item["guid"] = view.UniqueId;
                    item["name"] = view.Name;
                    item["view_type"] = view.ViewType.ToString();
                    item["scale"] = view.Scale;
                    string discipline = "Unknown";
                    item["discipline"] = discipline;
                    item["detail_level"] = view.DetailLevel.ToString();
                    viewports.TryGetValue(view.Id.IntegerValue, out int? sheetId);
                    item["associated_sheet_id"] = sheetId;
                    item["doc_id"] = doc.PathName;
                    result.Add(item);
                }
            }

            ModelCache.Set(doc.PathName + "/views", lastSaved, result);
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
