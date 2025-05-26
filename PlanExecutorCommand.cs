// PlanExecutorCommand.cs - Executes multi-step plans sent from AI agents
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

public class PlanExecutorCommand : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var response = new Dictionary<string, object>();
        var context = new Dictionary<string, object>();
        var doc = app.ActiveUIDocument.Document;
        var results = new List<object>();

        try
        {
            if (!input.ContainsKey("steps"))
            {
                response["status"] = "error";
                response["message"] = "Missing 'steps' field in input.";
                return response;
            }

            List<Dictionary<string, object>> steps;

            var rawSteps = input["steps"];
            if (rawSteps.TrimStart().StartsWith("["))
            {
                // steps is already a JSON array
                steps = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(rawSteps);
            }
            else
            {
                // try to parse as embedded JSON array inside string
                steps = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(JsonConvert.DeserializeObject<string>(rawSteps));
            }

            foreach (var step in steps)
            {
                if (!step.ContainsKey("action"))
                    continue;

                string action = step["action"].ToString();
                var paramDict = step.ContainsKey("params")
                    ? JsonConvert.DeserializeObject<Dictionary<string, string>>(step["params"].ToString())
                    : new Dictionary<string, string>();

                if (context.ContainsKey("elements"))
                    paramDict["input_elements"] = JsonConvert.SerializeObject(context["elements"]);

                if (RequestHandler.CommandMap.ContainsKey(action))
                {
                    var result = RequestHandler.CommandMap[action].Execute(app, paramDict);
                    if (result.ContainsKey("elements"))
                        context["elements"] = result["elements"];
                    results.Add(result);
                }
                else
                {
                    results.Add(new Dictionary<string, object> {
                        { "status", "error" },
                        { "message", $"Unknown action: {action}" }
                    });
                }
            }

            response["status"] = "success";
            response["results"] = results;
        }
        catch (Exception ex)
        {
            response["status"] = "error";
            response["message"] = ex.Message;
        }

        return response;
    }
}
