// ListCategoryParametersCommand.cs - Lists parameter names available for a category
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class ListCategoryParametersCommand : ICommand
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

        if (!input.TryGetValue("category", out var categoryName) || string.IsNullOrWhiteSpace(categoryName))
        {
            response["status"] = "error";
            response["message"] = "Provide a category name using the 'category' input.";
            return response;
        }
        categoryName = categoryName.Trim();

        BuiltInCategory builtInCategory;
        try
        {
            builtInCategory = CategoryUtils.ParseBuiltInCategory(categoryName);
        }
        catch (Exception ex)
        {
            response["status"] = "error";
            response["message"] = $"Unable to parse category '{categoryName}': {ex.Message}";
            return response;
        }

        DateTime lastSaved = System.IO.File.GetLastWriteTime(doc.PathName);
        var parameterCategories = ParameterMetadataCache.GetParameterCategories(doc, lastSaved);

        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (parameterCategories != null)
        {
            foreach (var kvp in parameterCategories)
            {
                if (kvp.Value == null || kvp.Value.Count == 0) continue;
                if (kvp.Value.Any(cat => cat.Equals(categoryName, StringComparison.OrdinalIgnoreCase)))
                    names.Add(kvp.Key);
            }
        }

        try
        {
            var element = new FilteredElementCollector(doc)
                .OfCategory(builtInCategory)
                .WhereElementIsNotElementType()
                .FirstElement();
            if (element != null)
            {
                foreach (Parameter param in element.Parameters)
                {
                    string name = param?.Definition?.Name;
                    if (!string.IsNullOrEmpty(name))
                        names.Add(name);
                }
            }
        }
        catch { }

        var orderedNames = names.OrderBy(n => n, StringComparer.OrdinalIgnoreCase).ToList();

        response["status"] = "success";
        response["category"] = categoryName;
        response["doc_id"] = doc.PathName;
        response["parameter_names"] = orderedNames;
        response["message"] = orderedNames.Count == 0
            ? "No parameters found for the specified category."
            : $"Found {orderedNames.Count} parameters for category '{categoryName}'.";

        return response;
    }
}
