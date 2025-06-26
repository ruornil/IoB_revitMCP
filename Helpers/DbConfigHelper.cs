// DbConfigHelper.cs - Loads PostgreSQL connection string for Revit MCP plugin
using System;
using System.Configuration;
using System.IO;
using System.Collections.Generic;

public static class DbConfigHelper
{
    /// <summary>
    /// Returns the PostgreSQL connection string from the first available source:
    /// 1. App.config (key: "revit")
    /// 2. Environment variable: REVIT_DB_CONN
    /// 3. File path defined in input dictionary: "conn_file"
    /// 4. Default local file: revit-conn.txt in plugin directory
    /// </summary>
    public static string GetConnectionString(Dictionary<string, string> input = null)
    {
        string conn = null;

        // 1. App.config
        try
        {
            conn = ConfigurationManager.ConnectionStrings["revit"]?.ConnectionString;
            if (!string.IsNullOrWhiteSpace(conn)) return conn.Trim();
        }
        catch { }

        // 2. Environment variable
        try
        {
            conn = Environment.GetEnvironmentVariable("REVIT_DB_CONN");
            if (!string.IsNullOrWhiteSpace(conn)) return conn.Trim();
        }
        catch { }

        // 3. Input-provided path
        try
        {
            if (input != null && input.TryGetValue("conn_file", out string pathFromInput))
            {
                if (File.Exists(pathFromInput))
                    return File.ReadAllText(pathFromInput).Trim();
            }
        }
        catch { }

        // 4. Default fallback path (next to DLL)
        try
        {
            string baseDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string fallbackPath = Path.Combine(baseDir, "revit-conn.txt");
            if (File.Exists(fallbackPath))
                return File.ReadAllText(fallbackPath).Trim();
        }
        catch { }

        return null;
    }

    /// <summary>
    /// Optionally inform AI or calling context where to place the connection file.
    /// </summary>
    public static string GetHelpMessage()
    {
        return "To enable database sync, please provide a valid PostgreSQL connection string. You can either:\n" +
               "1. Add it to App.config under <connectionStrings> with key 'revit'\n" +
               "2. Set environment variable REVIT_DB_CONN\n" +
               "3. Provide a file path using 'conn_file' input key\n" +
               "4. Place 'revit-conn.txt' in the plugin DLL folder";
    }
}
