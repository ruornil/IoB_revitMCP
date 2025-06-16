// CategoryUtils.cs - Helper utilities for categories
using System;
using Autodesk.Revit.DB;

public static class CategoryUtils
{
    public static BuiltInCategory ParseBuiltInCategory(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be null or empty", nameof(name));

        if (!name.StartsWith("OST_", StringComparison.OrdinalIgnoreCase))
            name = "OST_" + name;

        return (BuiltInCategory)Enum.Parse(typeof(BuiltInCategory), name, true);
    }
}

