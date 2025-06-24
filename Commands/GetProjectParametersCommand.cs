// GetProjectParametersCommand.cs - Retrieves all project parameters and their metadata
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class GetProjectParametersCommand : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var response = new Dictionary<string, object>();
        var doc = app?.ActiveUIDocument?.Document;

        if (doc == null)
        {
            response["status"] = "error";
            response["message"] = "No active document.";
            return response;
        }

        try
        {
            var bindingMap = doc.ParameterBindings;
            var iterator = bindingMap.ForwardIterator();
            var result = new List<Dictionary<string, object>>();

            iterator.Reset();
            while (iterator.MoveNext())
            {
                Definition definition = iterator.Key;
                ElementBinding binding = iterator.Current as ElementBinding;

                if (definition == null || binding == null) continue;

                var paramData = new Dictionary<string, object>();
                paramData["name"] = definition.Name;
                paramData["parameter_type"] = definition.ParameterGroup.ToString(); // fallback for Revit 2020-2021
                paramData["unit_type_id"] = definition.GetDataType()?.TypeId?.ToString() ?? "";
                paramData["is_instance"] = binding is InstanceBinding;

                // categories
                var categories = new List<string>();
                foreach (Category cat in binding.Categories)
                {
                    if (cat != null)
                        categories.Add(cat.Name);
                }
                paramData["categories"] = categories;

                result.Add(paramData);
            }

            response["status"] = "success";
            response["parameters"] = result;
        }
        catch (Exception ex)
        {
            response["status"] = "error";
            response["message"] = ex.Message;
        }

        return response;
    }
}
