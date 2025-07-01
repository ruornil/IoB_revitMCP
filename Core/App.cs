// App.cs - Entry point
using System;
using Autodesk.Revit.UI;

public class App : IExternalApplication
{
    public Result OnStartup(UIControlledApplication app)
    {
        try
        {
            McpServer.Start();
            return Result.Succeeded;
        }
        catch (Exception ex)
        {
            TaskDialog.Show("Startup Error", $"McpServer failed: {ex.Message}");
            return Result.Failed;
        }
    }

    public Result OnShutdown(UIControlledApplication app)
    {
        McpServer.Stop();
        QueueProcessor.Stop();
        return Result.Succeeded;
    }
}