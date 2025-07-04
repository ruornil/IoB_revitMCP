using Autodesk.Revit.UI;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;

public class QuerySqlCommand : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var response = new Dictionary<string, object>();

        try
        {
            string conn = DbConfigHelper.GetConnectionString(input);
            if (string.IsNullOrEmpty(conn))
            {
                response["status"] = "error";
                response["message"] = "No connection string found. " + DbConfigHelper.GetHelpMessage();
                return response;
            }

            if (!input.TryGetValue("sql", out string sql) || string.IsNullOrWhiteSpace(sql))
            {
                response["status"] = "error";
                response["message"] = "Missing 'sql' parameter.";
                return response;
            }

            NpgsqlParameter[] parameters = null;
            if (input.TryGetValue("params", out string paramJson) && !string.IsNullOrWhiteSpace(paramJson))
            {
                try
                {
                    var paramDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(paramJson);
                    var list = new List<NpgsqlParameter>();
                    foreach (var kvp in paramDict)
                    {
                        list.Add(new NpgsqlParameter(kvp.Key, kvp.Value ?? DBNull.Value));
                    }
                    parameters = list.ToArray();
                }
                catch (Exception ex)
                {
                    response["status"] = "error";
                    response["message"] = "Failed to parse params: " + ex.Message;
                    return response;
                }
            }

            var db = new BatchedPostgresDb(conn);
            var results = db.Query(sql, parameters);
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
