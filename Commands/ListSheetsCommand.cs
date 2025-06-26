using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class ListSheetsCommand : ICommand
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

        var sheets = new List<Dictionary<string, object>>();
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

            var col = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).Cast<ViewSheet>();
            foreach (var sheet in col)
            {
                var item = new Dictionary<string, object>();
                item["id"] = sheet.Id.IntegerValue;
                item["guid"] = sheet.UniqueId;
                item["name"] = sheet.Name;
                item["number"] = sheet.SheetNumber;
                var tbId = sheet.GetTypeId();
                Element tb = doc.GetElement(tbId);
                item["title_block"] = tb != null ? tb.Name : string.Empty;
                item["doc_id"] = doc.PathName;
                sheets.Add(item);

                db.UpsertSheet(sheet.Id.IntegerValue, Guid.Empty, sheet.Name, sheet.SheetNumber, item["title_block"].ToString(), doc.PathName);
            }
            response["status"] = "success";
            response["sheets"] = sheets;
        }
        catch (Exception ex)
        {
            response["status"] = "error";
            response["message"] = ex.Message;
        }
        return response;
    }
}
