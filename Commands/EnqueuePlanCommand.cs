using Autodesk.Revit.UI;
using Newtonsoft.Json;
using System.Collections.Generic;

/// <summary>
/// Inserts a plan into the mcp_queue table for asynchronous execution.
/// Returns the generated job id.
/// </summary>
public class EnqueuePlanCommand : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var response = new Dictionary<string, object>();
        string conn = DbConfigHelper.GetConnectionString(input);
        if (string.IsNullOrEmpty(conn) || !input.TryGetValue("plan", out var plan))
        {
            response["status"] = "error";
            response["message"] = "Missing connection string or plan";
            return response;
        }

        var db = new BatchedPostgresDb(conn);
        int id = db.EnqueuePlan(plan);
        response["status"] = "queued";
        response["job_id"] = id;
        return response;
    }
}
