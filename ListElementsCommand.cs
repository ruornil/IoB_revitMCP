// ListElementsCommand.cs - example of a typed commandset handler
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class ListElementsCommand : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var doc = app.ActiveUIDocument.Document;
        var response = new Dictionary<string, object>();
        string categoryName = input.ContainsKey("category") ? input["category"] : "Walls";

        var collector = new FilteredElementCollector(doc)
            .OfCategory((BuiltInCategory)Enum.Parse(typeof(BuiltInCategory), "OST_" + categoryName, true))
            .WhereElementIsNotElementType();

        var elements = collector.Select(e => new { Id = e.Id.IntegerValue, Name = e.Name }).ToList();
        response["status"] = "success";
        response["elements"] = elements;
        return response;
    }
}