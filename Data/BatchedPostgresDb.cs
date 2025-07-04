// Batching enhancement for Revit to PostgreSQL export with safe chunked batching, including element types
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BatchedPostgresDb : PostgresDb, IDisposable
{
    private NpgsqlConnection _conn;
    private NpgsqlTransaction _tx;
    private readonly string _connStr;

    private const int ChunkSize = 1000;
    private readonly List<string> _elementSqls = new List<string>();
    private readonly HashSet<int> _elementIds = new HashSet<int>();
    private class ParameterRow
    {
        public int ElementId;
        public string Name;
        public string Value;
        public bool IsType;
        public string[] Categories;
        public DateTime LastSaved;
    }

    private class TypeParameterRow
    {
        public int TypeId;
        public string Name;
        public string Value;
        public string[] Categories;
        public DateTime LastSaved;
    }

    private readonly Dictionary<string, ParameterRow> _paramSqlMap = new Dictionary<string, ParameterRow>();
    private readonly Dictionary<string, TypeParameterRow> _typeParamSqlMap = new Dictionary<string, TypeParameterRow>();
    private readonly List<string> _elementTypeSqls = new List<string>();
    private readonly HashSet<int> _elementTypeIds = new HashSet<int>();


    public BatchedPostgresDb(string connectionString) : base(connectionString)
    {
        _connStr = connectionString;
    }

    private void LogError(string context, Exception ex)
    {
        string msg = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR in {context}:\n{ex}\n";
        System.IO.File.AppendAllText("revit-export-error.log", msg);
    }
    private void EnsureTransaction()
    {
        if (_conn != null) return;

        if (string.IsNullOrWhiteSpace(_connStr))
            throw new InvalidOperationException("PostgreSQL connection string is empty or null.");

        try
        {
            _conn = new NpgsqlConnection(_connStr);
            _conn.Open();
            _tx = _conn.BeginTransaction();

            // Optional: Print a message to Revit for debugging
            // Autodesk.Revit.UI.TaskDialog.Show("DB Connection", "Connection to PostgreSQL established.");
        }
        catch (Exception ex)
        {
            string error = $"Failed to open PostgreSQL connection.\n\nConnection String:\n{_connStr}\n\nError:\n{ex.Message}";
            Console.WriteLine(error);

            // Optional: Show in Revit if running in UI context
            // Autodesk.Revit.UI.TaskDialog.Show("Connection Error", error);

            throw new ApplicationException("Could not establish PostgreSQL connection. See inner exception for details.", ex);
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
        string key = elementId + "::" + name;
        _paramSqlMap[key] = new ParameterRow
        {
            ElementId = elementId,
            Name = name,
            Value = value,
            IsType = isType,
            Categories = applicable,
            LastSaved = lastSaved
        };

        if (_paramSqlMap.Count >= ChunkSize) FlushParameters();
    }

    public void StageTypeParameter(int typeId, string name, string value, string[] applicable, DateTime lastSaved)
    {
        EnsureTransaction();
        string key = typeId + "::" + name;
        _typeParamSqlMap[key] = new TypeParameterRow
        {
            TypeId = typeId,
            Name = name,
            Value = value,
            Categories = applicable,
            LastSaved = lastSaved
        };

        if (_typeParamSqlMap.Count >= ChunkSize) FlushTypeParameters();
            FlushElementTypes();
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
            LogError("FlushParameters", ex);
            Console.WriteLine("Error flushing parameters: " + ex.Message);
            throw;
        }
        _elementSqls.Clear();
        _elementIds.Clear();
    }

    private void FlushParameters()
    {
        if (_paramSqlMap.Count == 0) return;
        try
        {
            var sb = new StringBuilder();
            var cmd = new NpgsqlCommand(string.Empty, _conn, _tx);
            int i = 0;
            foreach (var row in _paramSqlMap.Values)
            {
                if (sb.Length > 0) sb.Append(",");
                sb.Append($"(@el{i}, @name{i}, @val{i}, @typ{i}, @cat{i}, @ts{i})");

                cmd.Parameters.AddWithValue($"@el{i}", row.ElementId);
                cmd.Parameters.AddWithValue($"@name{i}", (object)row.Name ?? DBNull.Value);
                cmd.Parameters.AddWithValue($"@val{i}", (object)row.Value ?? DBNull.Value);
                cmd.Parameters.AddWithValue($"@typ{i}", row.IsType);
                cmd.Parameters.AddWithValue($"@cat{i}", (object)row.Categories ?? DBNull.Value);
                cmd.Parameters.AddWithValue($"@ts{i}", row.LastSaved);
                i++;
            }

            cmd.CommandText = "INSERT INTO revit_parameters (element_id, param_name, param_value, is_type, applicable_categories, last_saved) VALUES "
                + sb.ToString() + @"
                ON CONFLICT (element_id, param_name) DO UPDATE SET
                    param_value = EXCLUDED.param_value,
                    is_type = EXCLUDED.is_type,
                    applicable_categories = EXCLUDED.applicable_categories,
                    last_saved = EXCLUDED.last_saved
                WHERE EXCLUDED.last_saved > revit_parameters.last_saved";

            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            LogError("FlushParameters", ex);
            Console.WriteLine("Error flushing parameters: " + ex.Message);
            throw;
        }
        _paramSqlMap.Clear();
    }

    private void FlushTypeParameters()
    {
        if (_typeParamSqlMap.Count == 0) return;
        try
        {
            var sb = new StringBuilder();
            var cmd = new NpgsqlCommand(string.Empty, _conn, _tx);
            int i = 0;
            foreach (var row in _typeParamSqlMap.Values)
            {
                if (sb.Length > 0) sb.Append(",");
                sb.Append($"(@tid{i}, @name{i}, @val{i}, @cat{i}, @ts{i})");

                cmd.Parameters.AddWithValue($"@tid{i}", row.TypeId);
                cmd.Parameters.AddWithValue($"@name{i}", (object)row.Name ?? DBNull.Value);
                cmd.Parameters.AddWithValue($"@val{i}", (object)row.Value ?? DBNull.Value);
                cmd.Parameters.AddWithValue($"@cat{i}", (object)row.Categories ?? DBNull.Value);
                cmd.Parameters.AddWithValue($"@ts{i}", row.LastSaved);
                i++;
            }

            cmd.CommandText = "INSERT INTO revit_type_parameters (element_type_id, param_name, param_value, applicable_categories, last_saved) VALUES "
                + sb.ToString() + @"
                ON CONFLICT (element_type_id, param_name) DO UPDATE SET
                    param_value = EXCLUDED.param_value,
                    applicable_categories = EXCLUDED.applicable_categories,
                    last_saved = EXCLUDED.last_saved
                WHERE EXCLUDED.last_saved > revit_type_parameters.last_saved";

            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            LogError("FlushParameters", ex);
            Console.WriteLine("Error flushing parameters: " + ex.Message);
            throw;
        }
        _typeParamSqlMap.Clear();
    }

    public void StageElementType(int id, Guid guid, string family, string typeName, string category, string docId, DateTime lastSaved)
    {
        EnsureTransaction();
        if (_elementTypeIds.Contains(id)) return;

        string esc(string s) => s != null ? "'" + s.Replace("'", "''") + "'" : "NULL";
        _elementTypeSqls.Add($"({id}, '{guid}', {esc(family)}, {esc(typeName)}, {esc(category)}, {esc(docId)}, '{lastSaved:O}')");
        _elementTypeIds.Add(id);

        if (_elementTypeSqls.Count >= ChunkSize) FlushElementTypes();
    }

    private void FlushElementTypes()
    {
        if (_elementTypeSqls.Count == 0) return;
        try
        {
            string sql = "INSERT INTO revit_elementtypes (id, guid, family, type_name, category, doc_id, last_saved) VALUES "
                + string.Join(",\n", _elementTypeSqls) + @"
                ON CONFLICT (id) DO UPDATE SET
                    guid = EXCLUDED.guid,
                    family = EXCLUDED.family,
                    type_name = EXCLUDED.type_name,
                    category = EXCLUDED.category,
                    doc_id = EXCLUDED.doc_id,
                    last_saved = EXCLUDED.last_saved
                WHERE EXCLUDED.last_saved > revit_elementtypes.last_saved";
            new NpgsqlCommand(sql, _conn, _tx).ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            LogError("FlushParameters", ex);
            Console.WriteLine("Error flushing parameters: " + ex.Message);
            throw;
        }
        _elementTypeSqls.Clear();
        _elementTypeIds.Clear();
    }

    public void CommitAll()
    {
        try
        {
            FlushElements();
            FlushElementTypes();
            FlushParameters();
            FlushTypeParameters();
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
        _tx?.Dispose();
        _conn?.Dispose();
    }
}
