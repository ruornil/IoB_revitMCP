using Npgsql;
using System;

/// <summary>
/// Extends <see cref="PostgresDb"/> with batching capabilities. SQL commands
/// are executed on a single transaction until <see cref="CommitAll"/> is called.
/// </summary>
public class BatchedPostgresDb : PostgresDb, IDisposable
{
    private NpgsqlConnection _conn;
    private NpgsqlTransaction _tx;
    private readonly string _connStr;

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
        base.UpsertElement(id, guid, name, category, typeName, level, docId, lastSaved, _conn, _tx);
    }

    public void StageParameter(int elementId, string name, string value, bool isType,
        string[] applicable, DateTime lastSaved)
    {
        EnsureTransaction();
        base.UpsertParameter(elementId, name, value, isType, applicable, lastSaved, _conn, _tx);
    }

    // Route other upserts through the same transaction when it exists
    public new void UpsertModelInfo(string docId, string modelName, Guid guid, DateTime lastSaved,
        string projectInfo = null, string projectParameters = null,
        NpgsqlConnection conn = null, NpgsqlTransaction tx = null)
    {
        EnsureTransaction();
        base.UpsertModelInfo(docId, modelName, guid, lastSaved, projectInfo, projectParameters, _conn, _tx);
    }

    public new void UpsertElementType(int id, Guid guid, string family, string typeName,
        string category, string docId, DateTime lastSaved,
        NpgsqlConnection conn = null, NpgsqlTransaction tx = null)
    {
        EnsureTransaction();
        base.UpsertElementType(id, guid, family, typeName, category, docId, lastSaved, _conn, _tx);
    }

    public new void UpsertCategory(string enumVal, string name, string group, string description, Guid guid, DateTime lastSaved,
        NpgsqlConnection conn = null, NpgsqlTransaction tx = null)
    {
        EnsureTransaction();
        base.UpsertCategory(enumVal, name, group, description, guid, lastSaved, _conn, _tx);
    }

    public new void UpsertView(int id, Guid guid, string name, string viewType, int scale,
        string discipline, string detail, int? sheetId, string docId, DateTime lastSaved,
        NpgsqlConnection conn = null, NpgsqlTransaction tx = null)
    {
        EnsureTransaction();
        base.UpsertView(id, guid, name, viewType, scale, discipline, detail, sheetId, docId, lastSaved, _conn, _tx);
    }

    public new void UpsertSheet(int id, Guid guid, string name, string number, string titleBlock, string docId, DateTime lastSaved,
        NpgsqlConnection conn = null, NpgsqlTransaction tx = null)
    {
        EnsureTransaction();
        base.UpsertSheet(id, guid, name, number, titleBlock, docId, lastSaved, _conn, _tx);
    }

    public new void UpsertSchedule(int id, Guid guid, string name, string category, string docId, DateTime lastSaved,
        NpgsqlConnection conn = null, NpgsqlTransaction tx = null)
    {
        EnsureTransaction();
        base.UpsertSchedule(id, guid, name, category, docId, lastSaved, _conn, _tx);
    }

    public new void UpsertFamily(string name, string familyType, string category, string guid, string docId, DateTime lastSaved,
        NpgsqlConnection conn = null, NpgsqlTransaction tx = null)
    {
        EnsureTransaction();
        base.UpsertFamily(name, familyType, category, guid, docId, lastSaved, _conn, _tx);
    }

    public void CommitAll()
    {
        if (_tx != null)
        {
            _tx.Commit();
            _tx.Dispose();
            _tx = null;
        }
        if (_conn != null)
        {
            _conn.Close();
            _conn.Dispose();
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
