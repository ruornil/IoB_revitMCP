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

            PostgresDb db = new PostgresDb(conn);

            foreach (Category cat in doc.Settings.Categories)
            {
                var item = new Dictionary<string, object>();
                item["enum"] = cat.Id.IntegerValue;
                item["name"] = cat.Name;
                item["group"] = cat.CategoryType.ToString();
                item["guid"] = cat.Id.IntegerValue.ToString();
                categories.Add(item);

                db.UpsertCategory(cat.Id.IntegerValue.ToString(), cat.Name, cat.CategoryType.ToString(), item["description"].ToString(), Guid.Empty);
            }
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
}
