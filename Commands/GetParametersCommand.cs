// GetParametersCommand.cs
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

public class GetParametersCommand : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var doc = app.ActiveUIDocument.Document;
        var uidoc = app.ActiveUIDocument;
        var response = new Dictionary<string, object>();

        try
        {
            if (uidoc.Selection.GetElementIds().Count == 0)
            {
                response["status"] = "error";
                response["message"] = "No element selected.";
                return response;
            }

            var selectedId = uidoc.Selection.GetElementIds().First();
            var element = doc.GetElement(selectedId);

            var result = new Dictionary<string, string>();
            foreach (Parameter param in element.Parameters)
            {
                string name = param.Definition?.Name ?? "(no name)";
                string value = param.AsValueString() ?? param.AsString() ?? param.AsInteger().ToString();
                result[name] = value;
            }

            response["status"] = "success";
            response["parameters"] = result;
        }
        catch (System.Exception ex)
        {
            response["status"] = "error";
            response["message"] = ex.Message;
        }

        return response;
    }
}