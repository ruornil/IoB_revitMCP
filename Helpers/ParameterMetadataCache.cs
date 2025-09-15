using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;

/// <summary>
/// Utility helpers for caching parameter metadata such as applicable categories.
/// </summary>
public static class ParameterMetadataCache
{
    /// <summary>
    /// Returns a map of parameter name to the list of categories it applies to.
    /// Results are cached per document and invalidated when the model timestamp changes.
    /// </summary>
    public static Dictionary<string, List<string>> GetParameterCategories(Document doc, DateTime lastSaved)
    {
        if (doc == null) throw new ArgumentNullException(nameof(doc));

        string cacheKey = doc.PathName + "/paramCats";
        if (!ModelCache.TryGet(cacheKey, lastSaved, out Dictionary<string, List<string>> categories) || categories == null)
        {
            categories = BuildParameterCategoryMap(doc);
            ModelCache.Set(cacheKey, lastSaved, categories);
        }
        return categories;
    }

    private static Dictionary<string, List<string>> BuildParameterCategoryMap(Document doc)
    {
        var map = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        BindingMap bindingMap = doc.ParameterBindings;
        if (bindingMap == null)
            return map;

        var iterator = bindingMap.ForwardIterator();
        iterator.Reset();
        while (iterator.MoveNext())
        {
            Definition definition = iterator.Key as Definition;
            if (definition == null) continue;

            ElementBinding binding = iterator.Current as ElementBinding;
            if (binding?.Categories == null) continue;

            var list = GetOrCreate(map, definition.Name);
            foreach (Category category in binding.Categories)
            {
                if (category?.Name == null) continue;
                if (!list.Exists(name => name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)))
                    list.Add(category.Name);
            }
        }

        return map;
    }

    private static List<string> GetOrCreate(Dictionary<string, List<string>> map, string paramName)
    {
        if (!map.TryGetValue(paramName, out var list))
        {
            list = new List<string>();
            map[paramName] = list;
        }
        return list;
    }
}
