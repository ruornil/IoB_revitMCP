// FilterByParameterCommand.cs - Filters elements by parameter value
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

public class FilterByParameterCommand : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var response = new Dictionary<string, object>();
        var doc = app.ActiveUIDocument.Document;
        var result = new List<Dictionary<string, object>>();

        try
        {
            if (!input.ContainsKey("param") || !input.ContainsKey("value"))
            {
                response["status"] = "error";
                response["message"] = "Missing 'param' or 'value' field.";
                return response;
            }

            string targetParam = input["param"];
            string targetValue = input["value"];

            List<Element> elements = null;
            if (input.ContainsKey("input_elements"))
            {
                var elementList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(input["input_elements"]);
                elements = elementList
                    .Select(dict => dict.ContainsKey("Id")
                        ? doc.GetElement(new ElementId(Convert.ToInt32(dict["Id"])))
                        : null)
                    .Where(e => e != null)
                    .ToList();
            }
            else
            {
                response["status"] = "error";
                response["message"] = "No elements to filter. Provide 'input_elements'.";
                return response;
            }

            foreach (var e in elements)
            {
                var param = e.LookupParameter(targetParam);
                if (param == null) continue;

                string value = param.AsValueString() ?? param.AsString() ?? param.AsInteger().ToString();

                if (value != null && value.Equals(targetValue, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(new Dictionary<string, object>
                    {
                        { "Id", e.Id.IntegerValue },
                        { "Name", e.Name }
                    });
                }
            }

            response["status"] = "success";
            response["elements"] = result;
        }
        catch (Exception ex)
        {
            response["status"] = "error";
            response["message"] = ex.Message;
        }

        return response;
    }
}
