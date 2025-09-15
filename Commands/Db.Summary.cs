using Autodesk.Revit.UI;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;

public class DbSummaryCommand : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var resp = new Dictionary<string, object>();
        try
        {
            string conn = DbConfigHelper.GetConnectionString(input);
            if (string.IsNullOrEmpty(conn))
            {
                resp["status"] = "error";
                resp["message"] = "No DB connection string. Configure REVIT_DB_CONN or provide conn_file.";
                return resp;
            }
            var doc = app.ActiveUIDocument?.Document;
            string docId = doc?.PathName;
            if (input.ContainsKey("doc_id") && !string.IsNullOrWhiteSpace(input["doc_id"]))
                docId = input["doc_id"];
            if (string.IsNullOrWhiteSpace(docId))
            {
                resp["status"] = "error";
                resp["message"] = "Missing doc_id (no active document and none provided).";
                return resp;
            }

            var db = new PostgresDb(conn);
            var infoRows = db.Query("SELECT model_name, last_saved FROM model_info WHERE doc_id=@doc LIMIT 1", new NpgsqlParameter("@doc", docId));
            var byCat = db.Query("SELECT category, COUNT(*) AS c FROM revit_elements WHERE doc_id=@doc GROUP BY category ORDER BY c DESC LIMIT 20", new NpgsqlParameter("@doc", docId));
            var typesByCat = db.Query("SELECT category, COUNT(DISTINCT type_name) AS c FROM revit_elements WHERE doc_id=@doc GROUP BY category ORDER BY c DESC LIMIT 20", new NpgsqlParameter("@doc", docId));
            var totalElems = db.Query("SELECT COUNT(*) AS c FROM revit_elements WHERE doc_id=@doc", new NpgsqlParameter("@doc", docId));
            var totalTypes = db.Query("SELECT COUNT(DISTINCT type_name) AS c FROM revit_elements WHERE doc_id=@doc", new NpgsqlParameter("@doc", docId));

            var payload = new Dictionary<string, object>
            {
                {"kind","model_summary"},
                {"doc_id", docId},
                {"model_name", infoRows.Count>0 && infoRows[0].ContainsKey("model_name") ? infoRows[0]["model_name"] : null},
                {"last_saved", infoRows.Count>0 && infoRows[0].ContainsKey("last_saved") ? infoRows[0]["last_saved"] : null},
                {"total_elements", totalElems.Count>0 ? totalElems[0]["c"] : 0},
                {"total_types", totalTypes.Count>0 ? totalTypes[0]["c"] : 0},
                {"by_category", byCat},
                {"types_by_category", typesByCat}
            };

            // push ui event
            try
            {
                var sql = "INSERT INTO ui_events (session_id, doc_id, event_type, payload) VALUES (@s, @d, @t, @p) RETURNING id";
                var args = new []{
                    new NpgsqlParameter("@s", (object)(input.ContainsKey("session_id")? input["session_id"] : null) ?? DBNull.Value),
                    new NpgsqlParameter("@d", (object)docId ?? DBNull.Value),
                    new NpgsqlParameter("@t", "model_summary"),
                    new NpgsqlParameter("@p", NpgsqlTypes.NpgsqlDbType.Jsonb){ Value = JsonConvert.SerializeObject(payload) }
                };
                db.Query(sql, args);
            }
            catch { }

            resp["status"] = "success";
            resp["summary"] = payload;
        }
        catch (Exception ex)
        {
            resp["status"] = "error";
            resp["message"] = ex.Message;
        }
        return resp;
    }
}

