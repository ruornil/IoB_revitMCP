// RevitHelpers.cs - Functional utilities for IronPython scripts
using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

public static class RevitHelpers
{
    public static List<Element> GetElementsByCategory(Document doc, string categoryName)
    {
        var bic = (BuiltInCategory)Enum.Parse(typeof(BuiltInCategory), "OST_" + categoryName);
        return new FilteredElementCollector(doc)
            .OfCategory(bic)
            .WhereElementIsNotElementType()
            .ToElements()
            .ToList();
    }

    public static void SetParameter(Element element, string paramName, string value)
    {
        var param = element.LookupParameter(paramName);
        if (param != null && !param.IsReadOnly)
        {
            param.Set(value);
        }
    }
}