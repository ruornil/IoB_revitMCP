using System;
using System.Collections.Concurrent;

/// <summary>
/// Simple in-memory cache keyed by document path and last saved timestamp.
/// Cached values expire automatically when model's last_saved changes.
/// </summary>
public static class ModelCache
{
    private static readonly ConcurrentDictionary<string, (DateTime saved, object data)> _cache
        = new ConcurrentDictionary<string, (DateTime, object)>();

    public static bool TryGet<T>(string docPath, DateTime lastSaved, out T value)
    {
        value = default(T);
        if (_cache.TryGetValue(docPath, out var entry) && entry.saved == lastSaved && entry.data is T t)
        {
            value = t;
            return true;
        }
        return false;
    }

    public static void Set(string docPath, DateTime lastSaved, object value)
    {
        _cache[docPath] = (lastSaved, value);
    }
}
