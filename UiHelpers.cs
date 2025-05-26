// UiHelpers.cs - Helper methods exposed to IronPython
using Autodesk.Revit.UI;

public static class UiHelpers
{
    public static void ShowTaskDialog(string title, string message)
    {
        TaskDialog.Show(title, message);
    }
}