using Autodesk.Revit.UI;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;

public class UiPushEventCommand : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var resp = new Dictionary<string, object>();
        try
        {
            string eventType = input.ContainsKey("event_type") ? input["event_type"] : null;
            string payload = input.ContainsKey("payload") ? input["payload"] : null;
            string session = input.ContainsKey("session_id") ? input["session_id"] : null;
            if (string.IsNullOrWhiteSpace(eventType) || string.IsNullOrWhiteSpace(payload))
            {
                resp["status"] = "error";
                resp["message"] = "Missing 'event_type' or 'payload'";
                return resp;
            }

            string conn = DbConfigHelper.GetConnectionString(input);
            if (string.IsNullOrEmpty(conn))
            {
                resp["status"] = "error";
                resp["message"] = "No DB connection for Ui.PushEvent. Configure REVIT_DB_CONN or provide conn_file.";
                return resp;
            }

            var doc = app.ActiveUIDocument?.Document;
            string docId = doc?.PathName;
            if (input.ContainsKey("doc_id") && !string.IsNullOrWhiteSpace(input["doc_id"]))
                docId = input["doc_id"];

            var db = new PostgresDb(conn);
            var sql = "INSERT INTO ui_events (session_id, doc_id, event_type, payload) VALUES (@s, @d, @t, @p) RETURNING id, created_at";
            var args = new[]
            {
                new NpgsqlParameter("@s", (object)session ?? DBNull.Value),
                new NpgsqlParameter("@d", (object)docId ?? DBNull.Value),
                new NpgsqlParameter("@t", eventType),
                new NpgsqlParameter("@p", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = payload }
            };
            var rows = db.Query(sql, args);
            resp["status"] = "success";
            resp["id"] = rows.Count > 0 && rows[0].ContainsKey("id") ? rows[0]["id"] : null;
            resp["created_at"] = rows.Count > 0 && rows[0].ContainsKey("created_at") ? rows[0]["created_at"] : null;
        }
        catch (Exception ex)
        {
            resp["status"] = "error";
            resp["message"] = ex.Message;
        }
        return resp;
    }
}

