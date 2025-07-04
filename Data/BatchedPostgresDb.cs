// Batching enhancement for Revit to PostgreSQL export with safe chunked batching
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

public class BatchedPostgresDb : PostgresDb, IDisposable
{
    private NpgsqlConnection _conn;
    private NpgsqlTransaction _tx;
    private readonly string _connStr;

    private const int ChunkSize = 1000;
    private readonly List<string> _elementSqls = new List<string>();
    private readonly HashSet<int> _elementIds = new HashSet<int>();
    private readonly Dictionary<string, string> _paramSqlMap = new Dictionary<string, string>();

    public BatchedPostgresDb(string connectionString) : base(connectionString)
    {
        _connStr = connectionString;
    }

    private void EnsureTransaction()
    {
        if (_conn == null)
        {
            _conn = new NpgsqlConnection(_connStr);
            _conn.Open();
            _tx = _conn.BeginTransaction();
        }
    }

    public void StageElement(int id, Guid guid, string name, string category,
        string typeName, string level, string docId, DateTime lastSaved)
    {
        EnsureTransaction();
        if (_elementIds.Contains(id)) return; // prevent duplicates

        string esc(string s) => s != null ? "'" + s.Replace("'", "''") + "'" : "NULL";
        _elementSqls.Add($"({id}, '{guid}', {esc(name)}, {esc(category)}, {esc(typeName)}, {esc(level)}, {esc(docId)}, '{lastSaved:O}')");
        _elementIds.Add(id);

        if (_elementSqls.Count >= ChunkSize) FlushElements();
    }

    public void StageParameter(int elementId, string name, string value, bool isType,
        string[] applicable, DateTime lastSaved)
    {
        EnsureTransaction();
        string esc(string s) => s != null ? "'" + s.Replace("'", "''") + "'" : "NULL";
        string cats = applicable != null ? $"ARRAY[{string.Join(",", applicable.Select(c => esc(c)))}]" : "NULL";
        string key = elementId + "::" + name;
        string sql = $"({elementId}, {esc(name)}, {esc(value)}, {(isType ? "TRUE" : "FALSE")}, {cats}, '{lastSaved:O}')";
        _paramSqlMap[key] = sql;

        if (_paramSqlMap.Count >= ChunkSize) FlushParameters();
    }

    private void FlushElements()
    {
        if (_elementSqls.Count == 0) return;
        try
        {
            string sql = "INSERT INTO revit_elements (id, guid, name, category, type_name, level, doc_id, last_saved) VALUES "
                + string.Join(",\n", _elementSqls) + @"
                ON CONFLICT (id) DO UPDATE SET
                    guid = EXCLUDED.guid,
                    name = EXCLUDED.name,
                    category = EXCLUDED.category,
                    type_name = EXCLUDED.type_name,
                    level = EXCLUDED.level,
                    doc_id = EXCLUDED.doc_id,
                    last_saved = EXCLUDED.last_saved
                WHERE EXCLUDED.last_saved > revit_elements.last_saved";
            new NpgsqlCommand(sql, _conn, _tx).ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error flushing elements: " + ex.Message);
            throw;
        }
        _elementSqls.Clear();
        _elementIds.Clear();
    }

    private void FlushParameters()
    {
        // Ensure all related elements are flushed before parameters to satisfy FK constraints
        if (_elementSqls.Count > 0)
            FlushElements();
        if (_paramSqlMap.Count == 0) return;
        try
        {
            string sql = "INSERT INTO revit_parameters (element_id, param_name, param_value, is_type, applicable_categories, last_saved) VALUES "
                + string.Join(",\n", _paramSqlMap.Values) + @"
                ON CONFLICT (element_id, param_name) DO UPDATE SET
                    param_value = EXCLUDED.param_value,
                    is_type = EXCLUDED.is_type,
                    applicable_categories = EXCLUDED.applicable_categories,
                    last_saved = EXCLUDED.last_saved
                WHERE EXCLUDED.last_saved > revit_parameters.last_saved";
            new NpgsqlCommand(sql, _conn, _tx).ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error flushing parameters: " + ex.Message);
            throw;
        }
        _paramSqlMap.Clear();
    }

    public void CommitAll()
    {
        try
        {
            FlushElements();
            FlushParameters();
            _tx?.Commit();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Commit failed: " + ex.Message);
            throw;
        }
        finally
        {
            _tx?.Dispose();
            _conn?.Close();
            _conn?.Dispose();
            _tx = null;
            _conn = null;
        }
    }

    public void Dispose()
    {
        if (_tx != null || _conn != null)
        {
            _tx?.Dispose();
            _conn?.Dispose();
        }
    }
}
