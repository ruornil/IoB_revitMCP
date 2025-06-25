// GetFamiliesAndTypesCommand.cs - lists all families and their types in the model
// Extended to include family GUID and document id
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class GetFamiliesAndTypesCommand : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var doc = app.ActiveUIDocument.Document;
        var response = new Dictionary<string, object>();
        var result = new List<Dictionary<string, object>>();

        try
        {
            Type filterType = typeof(ElementType);

            if (input.ContainsKey("class_name"))
            {
                string className = input["class_name"];
                var revitApiAssembly = typeof(FamilyInstance).Assembly;
                var resolved = revitApiAssembly.GetTypes().FirstOrDefault(t => t.Name == className);

                if (resolved != null && typeof(ElementType).IsAssignableFrom(resolved))
                {
                    filterType = resolved;
                }
                else
                {
                    response["status"] = "error";
                    response["message"] = $"'{className}' is not a valid Revit ElementType class.";
                    return response;
                }
            }

            var types = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .OfClass(filterType)
                .Cast<ElementType>();

            foreach (var type in types)
            {
                if (type.FamilyName != null && type.Name != null)
                {
                    var item = new Dictionary<string, object>();
                    item["family"] = type.FamilyName;
                    item["type"] = type.Name;
                    item["id"] = type.Id.IntegerValue.ToString();
                    item["category"] = type.Category?.Name ?? string.Empty;
                    item["guid"] = type.UniqueId;
                    item["doc_id"] = doc.PathName;
                    result.Add(item);
                }
            }

            response["status"] = "success";
            response["types"] = result;
        }
        catch (Exception ex)
        {
            response["status"] = "error";
            response["message"] = ex.Message;
        }

        return response;
    }
}
