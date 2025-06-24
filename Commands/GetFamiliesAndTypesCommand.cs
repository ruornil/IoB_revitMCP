// GetFamiliesAndTypesCommand.cs - lists all families and their types in the model optionally filtered by Revit class
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
        var result = new List<Dictionary<string, string>>();

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
                    result.Add(new Dictionary<string, string>
                    {
                        { "family", type.FamilyName },
                        { "type", type.Name },
                        { "id", type.Id.IntegerValue.ToString() },
                        { "category", type.Category?.Name ?? "" }
                    });
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
