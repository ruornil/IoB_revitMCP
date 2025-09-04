using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class ListCategoriesCommand : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var response = new Dictionary<string, object>();
        var doc = app.ActiveUIDocument?.Document;
        bool includeLinked = input.TryGetValue("include_linked", out var inc) && inc.Equals("true", StringComparison.OrdinalIgnoreCase);
        if (doc == null)
        {
            response["status"] = "error";
            response["message"] = "No active document.";
            return response;
        }

        var categories = new List<Dictionary<string, object>>();
        try
        {
            string conn = DbConfigHelper.GetConnectionString(input);
            if (string.IsNullOrEmpty(conn))
            {
                response["status"] = "error";
                response["message"] = "No connection string found. " + DbConfigHelper.GetHelpMessage();
                return response;
            }

            BatchedPostgresDb db = new BatchedPostgresDb(conn);
            DateTime lastSaved = System.IO.File.GetLastWriteTime(doc.PathName);
            if (db.GetModelLastSaved(doc.PathName) == lastSaved)
            {
                response["status"] = "up_to_date";
                return response;
            }

            foreach (Category cat in doc.Settings.Categories)
            {
                var item = new Dictionary<string, object>();
                item["enum"] = cat.Id.IntegerValue;
                item["name"] = cat.Name;
                item["group"] = cat.CategoryType.ToString();
                item["guid"] = cat.Id.IntegerValue.ToString();
                item["description"] = string.Empty;
                item["doc_id"] = doc.PathName;
                categories.Add(item);

                db.UpsertCategory(cat.Id.IntegerValue.ToString(), cat.Name, cat.CategoryType.ToString(), item["description"].ToString(), Guid.Empty, lastSaved);
            }

            if (includeLinked)
            {
                var links = new FilteredElementCollector(doc)
                    .OfClass(typeof(RevitLinkInstance))
                    .Cast<RevitLinkInstance>();
                foreach (var link in links)
                {
                    Document linkDoc = null;
                    try { linkDoc = link.GetLinkDocument(); } catch { linkDoc = null; }
                    if (linkDoc == null) continue;

                    foreach (Category cat in linkDoc.Settings.Categories)
                    {
                        var item = new Dictionary<string, object>();
                        item["enum"] = cat.Id.IntegerValue;
                        item["name"] = cat.Name;
                        item["group"] = cat.CategoryType.ToString();
                        item["guid"] = cat.Id.IntegerValue.ToString();
                        item["description"] = string.Empty;
                        item["doc_id"] = linkDoc.PathName;
                        item["source"] = "link";
                        item["link_instance_id"] = link.Id.IntegerValue;
                        categories.Add(item);
                    }
                }
            }
            db.UpsertModelInfo(doc.PathName, doc.Title, ParseGuid(doc.ProjectInformation.UniqueId), lastSaved,
                null, null);
            db.CommitAll();
            response["status"] = "success";
            response["categories"] = categories;
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
