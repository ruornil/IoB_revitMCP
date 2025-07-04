using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;

public class CaptureToolStateCommand : ICommand
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

        try
        {
            var state = new Dictionary<string, object>();
            state["document_name"] = doc.Title;

            var view = doc.ActiveView;
            var activeView = new Dictionary<string, object>
            {
                ["name"] = view.Name,
                ["id"] = view.Id.IntegerValue,
                ["type"] = view.ViewType.ToString()
            };
            state["active_view"] = activeView;

            var selected = new List<Dictionary<string, object>>();
            foreach (ElementId id in uidoc.Selection.GetElementIds())
            {
                var el = doc.GetElement(id);
                if (el == null) continue;

                var item = new Dictionary<string, object>();
                item["id"] = el.Id.IntegerValue;
                item["category"] = el.Category?.Name ?? string.Empty;
                item["type_name"] = doc.GetElement(el.GetTypeId())?.Name ?? string.Empty;

                var paramDict = new Dictionary<string, string>();
                foreach (Parameter p in el.Parameters)
                {
                    if (p.Definition?.Name == null) continue;
                    string val = p.StorageType switch
                    {
                        StorageType.String => p.AsString(),
                        StorageType.Integer => p.AsInteger().ToString(),
                        StorageType.Double => p.AsDouble().ToString(),
                        StorageType.ElementId => p.AsElementId().IntegerValue.ToString(),
                        _ => string.Empty
                    };
                    paramDict[p.Definition.Name] = val ?? string.Empty;
                }
                item["parameters"] = paramDict;

                selected.Add(item);
            }
            state["selected_elements"] = selected;

            response["status"] = "success";
            response["tool_state"] = state;
        }
        catch (Exception ex)
        {
            response["status"] = "error";
            response["message"] = ex.Message;
        }

        return response;
    }
}
