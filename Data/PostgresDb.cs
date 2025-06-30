using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Minimal PostgreSQL helper for executing commands and queries.
/// Connection string should be provided via constructor.
/// </summary>
public class PostgresDb
{
    private readonly string _connectionString;

    public PostgresDb(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int ExecuteNonQuery(string sql, params NpgsqlParameter[] args)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        using (var cmd = new NpgsqlCommand(sql, conn))
        {
            if (args != null)
                cmd.Parameters.AddRange(args);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }
    }

    public List<Dictionary<string, object>> Query(string sql, params NpgsqlParameter[] args)
    {
        var results = new List<Dictionary<string, object>>();
        using (var conn = new NpgsqlConnection(_connectionString))
        using (var cmd = new NpgsqlCommand(sql, conn))
        {
            if (args != null)
                cmd.Parameters.AddRange(args);
            conn.Open();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader.GetValue(i);
                    }
                    results.Add(row);
                }
            }
        }
        return results;
    }

    public void UpsertElement(int id, Guid guid, string name, string category,
        string typeName, string level, string docId, DateTime lastSeen)
    {
        string sql = @"INSERT INTO revit_elements
            (id, guid, name, category, type_name, level, doc_id, last_seen)
            VALUES (@id, @guid, @name, @category, @type_name, @level, @doc_id, @last_seen)
            ON CONFLICT (id) DO UPDATE SET
                guid = EXCLUDED.guid,
                name = EXCLUDED.name,
                category = EXCLUDED.category,
                type_name = EXCLUDED.type_name,
                level = EXCLUDED.level,
                doc_id = EXCLUDED.doc_id,
                last_seen = EXCLUDED.last_seen";

        ExecuteNonQuery(sql,
            new NpgsqlParameter("@id", id),
            new NpgsqlParameter("@guid", guid),
            new NpgsqlParameter("@name", name ?? (object)DBNull.Value),
            new NpgsqlParameter("@category", category ?? (object)DBNull.Value),
            new NpgsqlParameter("@type_name", typeName ?? (object)DBNull.Value),
            new NpgsqlParameter("@level", level ?? (object)DBNull.Value),
            new NpgsqlParameter("@doc_id", docId ?? (object)DBNull.Value),
            new NpgsqlParameter("@last_seen", lastSeen));
    }

    public void UpsertParameter(int elementId, string name, string value, bool isType,
        string[] applicable, DateTime lastSaved)
    {
        string sql = @"INSERT INTO revit_parameters
            (element_id, param_name, param_value, is_type, applicable_categories, last_saved)
            VALUES (@elid, @name, @value, @is_type, @categories, @last_saved)
            ON CONFLICT (element_id, param_name) DO UPDATE SET
                param_value = EXCLUDED.param_value,
                is_type = EXCLUDED.is_type,
                applicable_categories = EXCLUDED.applicable_categories,
                last_saved = EXCLUDED.last_saved";

        ExecuteNonQuery(sql,
            new NpgsqlParameter("@elid", elementId),
            new NpgsqlParameter("@name", name),
            new NpgsqlParameter("@value", value ?? (object)DBNull.Value),
            new NpgsqlParameter("@is_type", isType),
            new NpgsqlParameter("@categories", applicable ?? (object)DBNull.Value),
            new NpgsqlParameter("@last_saved", lastSaved));
    }

    public void UpsertCategory(string enumVal, string name, string group, string description, Guid guid, DateTime lastSaved)
    {
        string sql = @"INSERT INTO revit_categories
            (enum, name, category_group, description, guid, last_saved)
            VALUES (@enum, @name, @group, @description, @guid, @last_saved)
            ON CONFLICT (enum) DO UPDATE SET
                name = EXCLUDED.name,
                category_group = EXCLUDED.category_group,
                description = EXCLUDED.description,
                guid = EXCLUDED.guid,
                last_saved = EXCLUDED.last_saved";

        ExecuteNonQuery(sql,
            new NpgsqlParameter("@enum", enumVal),
            new NpgsqlParameter("@name", name ?? (object)DBNull.Value),
            new NpgsqlParameter("@group", group ?? (object)DBNull.Value),
            new NpgsqlParameter("@description", description ?? (object)DBNull.Value),
            new NpgsqlParameter("@guid", guid),
            new NpgsqlParameter("@last_saved", lastSaved));
    }

    public void UpsertView(int id, Guid guid, string name, string viewType, int scale,
        string discipline, string detail, int? sheetId, string docId, DateTime lastSaved)
    {
        string sql = @"INSERT INTO revit_views
            (id, guid, name, view_type, scale, discipline, detail_level, associated_sheet_id, doc_id, last_saved)
            VALUES (@id, @guid, @name, @type, @scale, @disc, @detail, @sheet, @doc, @last_saved)
            ON CONFLICT (id) DO UPDATE SET
                guid = EXCLUDED.guid,
                name = EXCLUDED.name,
                view_type = EXCLUDED.view_type,
                scale = EXCLUDED.scale,
                discipline = EXCLUDED.discipline,
                detail_level = EXCLUDED.detail_level,
                associated_sheet_id = EXCLUDED.associated_sheet_id,
                doc_id = EXCLUDED.doc_id,
                last_saved = EXCLUDED.last_saved";

        ExecuteNonQuery(sql,
            new NpgsqlParameter("@id", id),
            new NpgsqlParameter("@guid", guid),
            new NpgsqlParameter("@name", name ?? (object)DBNull.Value),
            new NpgsqlParameter("@type", viewType ?? (object)DBNull.Value),
            new NpgsqlParameter("@scale", scale),
            new NpgsqlParameter("@disc", discipline ?? (object)DBNull.Value),
            new NpgsqlParameter("@detail", detail ?? (object)DBNull.Value),
            new NpgsqlParameter("@sheet", sheetId ?? (object)DBNull.Value),
            new NpgsqlParameter("@doc", docId ?? (object)DBNull.Value),
            new NpgsqlParameter("@last_saved", lastSaved));
    }

    public void UpsertSheet(int id, Guid guid, string name, string number, string titleBlock, string docId, DateTime lastSaved)
    {
        string sql = @"INSERT INTO revit_sheets
            (id, guid, name, number, title_block, doc_id, last_saved)
            VALUES (@id, @guid, @name, @num, @tb, @doc, @last_saved)
            ON CONFLICT (id) DO UPDATE SET
                guid = EXCLUDED.guid,
                name = EXCLUDED.name,
                number = EXCLUDED.number,
                title_block = EXCLUDED.title_block,
                doc_id = EXCLUDED.doc_id,
                last_saved = EXCLUDED.last_saved";

        ExecuteNonQuery(sql,
            new NpgsqlParameter("@id", id),
            new NpgsqlParameter("@guid", guid),
            new NpgsqlParameter("@name", name ?? (object)DBNull.Value),
            new NpgsqlParameter("@num", number ?? (object)DBNull.Value),
            new NpgsqlParameter("@tb", titleBlock ?? (object)DBNull.Value),
            new NpgsqlParameter("@doc", docId ?? (object)DBNull.Value),
            new NpgsqlParameter("@last_saved", lastSaved));
    }

    public void UpsertSchedule(int id, Guid guid, string name, string category, string docId, DateTime lastSaved)
    {
        string sql = @"INSERT INTO revit_schedules
            (id, guid, name, category, doc_id, last_saved)
            VALUES (@id, @guid, @name, @cat, @doc, @last_saved)
            ON CONFLICT (id) DO UPDATE SET
                guid = EXCLUDED.guid,
                name = EXCLUDED.name,
                category = EXCLUDED.category,
                doc_id = EXCLUDED.doc_id,
                last_saved = EXCLUDED.last_saved";

        ExecuteNonQuery(sql,
            new NpgsqlParameter("@id", id),
            new NpgsqlParameter("@guid", guid),
            new NpgsqlParameter("@name", name ?? (object)DBNull.Value),
            new NpgsqlParameter("@cat", category ?? (object)DBNull.Value),
            new NpgsqlParameter("@doc", docId ?? (object)DBNull.Value),
            new NpgsqlParameter("@last_saved", lastSaved));
    }

    public void UpsertFamily(string name, string familyType, string category, string guid, string docId, DateTime lastSaved)
    {
        string sql = @"INSERT INTO revit_families
            (name, family_type, category, guid, doc_id, last_saved)
            VALUES (@name, @type, @cat, @guid, @doc, @last_saved)
            ON CONFLICT (name, family_type, category) DO UPDATE SET
                guid = EXCLUDED.guid,
                doc_id = EXCLUDED.doc_id,
                last_saved = EXCLUDED.last_saved";

        ExecuteNonQuery(sql,
            new NpgsqlParameter("@name", name ?? (object)DBNull.Value),
            new NpgsqlParameter("@type", familyType ?? (object)DBNull.Value),
            new NpgsqlParameter("@cat", category ?? (object)DBNull.Value),
            new NpgsqlParameter("@guid", guid ?? (object)DBNull.Value),
            new NpgsqlParameter("@doc", docId ?? (object)DBNull.Value),
            new NpgsqlParameter("@last_saved", lastSaved));
    }

    public void UpsertElementType(int id, Guid guid, string family, string typeName,
        string category, string docId, DateTime lastSeen)
    {
        string sql = @"INSERT INTO revit_elementTypes
            (id, guid, family, type_name, category, doc_id, last_seen)
            VALUES (@id, @guid, @family, @type_name, @category, @doc_id, @last_seen)
            ON CONFLICT (id) DO UPDATE SET
                guid = EXCLUDED.guid,
                family = EXCLUDED.family,
                type_name = EXCLUDED.type_name,
                category = EXCLUDED.category,
                doc_id = EXCLUDED.doc_id,
                last_seen = EXCLUDED.last_seen";

        ExecuteNonQuery(sql,
            new NpgsqlParameter("@id", id),
            new NpgsqlParameter("@guid", guid),
            new NpgsqlParameter("@family", family ?? (object)DBNull.Value),
            new NpgsqlParameter("@type_name", typeName ?? (object)DBNull.Value),
            new NpgsqlParameter("@category", category ?? (object)DBNull.Value),
            new NpgsqlParameter("@doc_id", docId ?? (object)DBNull.Value),
            new NpgsqlParameter("@last_seen", lastSeen));
    }

    public void UpsertModelInfo(string docId, string modelName, Guid guid, DateTime lastSaved, string projectInfo = null, string projectParameters = null)
    {
        string sql = @"INSERT INTO model_info
            (doc_id, model_name, guid, last_saved, project_info, project_parameters)
            VALUES (@doc, @name, @guid, @last_saved, @info, @params)
            ON CONFLICT (doc_id) DO UPDATE SET
                model_name = EXCLUDED.model_name,
                guid = EXCLUDED.guid,
                last_saved = EXCLUDED.last_saved,
                project_info = EXCLUDED.project_info,
                project_parameters = EXCLUDED.project_parameters";

        var infoParam = new NpgsqlParameter("@info", projectInfo ?? (object)DBNull.Value)
        {
            NpgsqlDbType = NpgsqlDbType.Jsonb
        };
        var paramsParam = new NpgsqlParameter("@params", projectParameters ?? (object)DBNull.Value)
        {
            NpgsqlDbType = NpgsqlDbType.Jsonb
        };

        ExecuteNonQuery(sql,
            new NpgsqlParameter("@doc", docId ?? (object)DBNull.Value),
            new NpgsqlParameter("@name", modelName ?? (object)DBNull.Value),
            new NpgsqlParameter("@guid", guid),
            new NpgsqlParameter("@last_saved", lastSaved),
            infoParam,
            paramsParam);
    }

    public DateTime? GetModelLastSaved(string docId)
    {
        var rows = Query("SELECT last_saved FROM model_info WHERE doc_id = @doc", new NpgsqlParameter("@doc", docId));
        if (rows.Count > 0 && rows[0].ContainsKey("last_saved") && rows[0]["last_saved"] is DateTime dt)
            return dt;
        return null;
    }
}
