// SetParameters.cs - Sets parameter values on Revit elements
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

public class SetParametersCommand : ICommand
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

        // Parse element IDs if provided
        var elementIds = new List<ElementId>();
        if (input.TryGetValue("element_ids", out var idJson))
        {
            try
            {
                var idList = JArray.Parse(idJson).Select(id => new ElementId((int)id));
                elementIds.AddRange(idList);
            }
            catch
            {
                response["status"] = "error";
                response["message"] = "Invalid element_ids format. Must be a JSON array of integers.";
                return response;
            }
        }
        else
        {
            elementIds = uidoc.Selection.GetElementIds().ToList();
        }

        if (!input.TryGetValue("parameters", out var paramBlock))
        {
            response["status"] = "error";
            response["message"] = "Missing 'parameters' block.";
            return response;
        }

        var paramDict = JObject.Parse(paramBlock);
        var updated = new List<int>();
        var skipped = new List<int>();

        using (var tx = new Transaction(doc, "Set Parameters"))
        {
            tx.Start();

            foreach (var id in elementIds)
            {
                var element = doc.GetElement(id);
                bool elementUpdated = false;

                foreach (var pair in paramDict)
                {
                    var param = element.LookupParameter(pair.Key);
                    if (param == null || param.IsReadOnly) continue;

                    bool success = false;
                    string value = pair.Value?.ToString();
                    switch (param.StorageType)
                    {
                        case StorageType.String:
                            success = param.Set(value);
                            break;
                        case StorageType.Integer:
                            if (int.TryParse(value, out var intVal))
                                success = param.Set(intVal);
                            break;
                        case StorageType.Double:
                            if (double.TryParse(value, out var dblVal))
                                success = param.Set(dblVal);
                            break;
                        case StorageType.ElementId:
                            if (int.TryParse(value, out var elid))
                                success = param.Set(new ElementId(elid));
                            break;
                    }

                    if (success) elementUpdated = true;
                }

                if (elementUpdated)
                    updated.Add(id.IntegerValue);
                else
                    skipped.Add(id.IntegerValue);
            }

            tx.Commit();
        }

        response["status"] = "success";
        response["updated"] = updated;
        response["skipped"] = skipped;
        return response;
    }
}
