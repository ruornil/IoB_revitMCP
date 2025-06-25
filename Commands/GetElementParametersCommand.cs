// GetElementParametersCommand.cs - Retrieves parameters for elements by ID or selection
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class GetElementParametersCommand : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var response = new Dictionary<string, object>();
        var uidoc = app.ActiveUIDocument;
        var doc = uidoc?.Document;

        if (doc == null)
        {
            response["status"] = "error";
            response["message"] = "No active document.";
            return response;
        }

        var ids = new List<ElementId>();
        if (input.TryGetValue("element_ids", out var idStr) && !string.IsNullOrWhiteSpace(idStr))
        {
            foreach (var part in idStr.Split(','))
            {
                if (int.TryParse(part.Trim(), out var intId))
                    ids.Add(new ElementId(intId));
            }
        }
        else
        {
            ids.AddRange(uidoc.Selection.GetElementIds());
        }

        if (ids.Count == 0)
        {
            response["status"] = "error";
            response["message"] = "No elements specified or selected.";
            return response;
        }

        var result = new Dictionary<string, object>();
        foreach (var id in ids)
        {
            var element = doc.GetElement(id);
            if (element == null)
            {
                result[id.IntegerValue.ToString()] = "Element not found.";
                continue;
            }

            var paramData = new Dictionary<string, object>();
            foreach (Parameter param in element.Parameters)
            {
                if (param == null) continue;
                string name = param.Definition?.Name;
                if (string.IsNullOrEmpty(name)) continue;
                string storage = param.StorageType.ToString();
                object value = null;
                switch (param.StorageType)
                {
                    case StorageType.String:
                        value = param.AsString();
                        break;
                    case StorageType.Double:
                        value = param.AsDouble();
                        break;
                    case StorageType.Integer:
                        value = param.AsInteger();
                        break;
                    case StorageType.ElementId:
                        value = param.AsElementId().IntegerValue;
                        break;
                }
                bool isType = param.Element != null && param.Element.Id != element.Id;
                var pInfo = new Dictionary<string, object>();
                pInfo["value"] = value;
                pInfo["storage"] = storage;
                pInfo["is_type"] = isType;
                paramData[name] = pInfo;
            }
            result[id.IntegerValue.ToString()] = paramData;
        }

        response["status"] = "success";
        response["parameters"] = result;
        return response;
    }
}
