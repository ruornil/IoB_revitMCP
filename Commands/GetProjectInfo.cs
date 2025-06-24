// GetModelInfoCommand.cs - Retrieves model-level metadata such as name, save time, and project info
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

public class GetProjectInfoCommand : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var doc = app?.ActiveUIDocument?.Document;
        var response = new Dictionary<string, object>();

        if (doc == null)
        {
            response["status"] = "error";
            response["message"] = "No active Revit document.";
            return response;
        }

        try
        {
            var projectInfo = doc.ProjectInformation;
            var info = new Dictionary<string, string>();

            foreach (Parameter p in projectInfo.Parameters)
            {
                if (p == null || string.IsNullOrEmpty(p.Definition?.Name)) continue;
                string val = "";
                if (p.StorageType == StorageType.String) val = p.AsString();
                else if (p.StorageType == StorageType.Integer) val = p.AsInteger().ToString();
                else if (p.StorageType == StorageType.Double) val = p.AsDouble().ToString();
                else if (p.StorageType == StorageType.ElementId) val = p.AsElementId().IntegerValue.ToString();
                info[p.Definition.Name] = val;
            }

            response["status"] = "success";
            response["model_name"] = doc.Title;
            response["last_saved"] = System.IO.File.GetLastWriteTime(doc.PathName).ToString("yyyy-MM-ddTHH:mm:ss");
            response["project_info"] = info;
        }
        catch (Exception ex)
        {
            response["status"] = "error";
            response["message"] = ex.Message;
        }

        return response;
    }
}
