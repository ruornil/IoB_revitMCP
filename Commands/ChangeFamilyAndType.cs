// ChangeFamilyAndType.cs - Updates both family and type of multiple Revit elements
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class ChangeFamilyAndType : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var response = new Dictionary<string, object>();
        var doc = app.ActiveUIDocument?.Document;

        if (doc == null || !input.TryGetValue("element_ids", out string elementIdsStr) ||
            !input.TryGetValue("new_type_name", out string newTypeName))
        {
            response["status"] = "error";
            response["message"] = "Missing required inputs: element_ids and new_type_name.";
            return response;
        }

        string[] idStrings = elementIdsStr.Split(',');
        List<string> changed = new List<string>();
        List<string> failed = new List<string>();

        ElementType newType = new FilteredElementCollector(doc)
            .OfClass(typeof(ElementType))
            .FirstOrDefault(e => e.Name == newTypeName && (e as ElementType).FamilyName != null) as ElementType;

        if (newType == null)
        {
            response["status"] = "error";
            response["message"] = $"No matching ElementType with name '{newTypeName}' found.";
            return response;
        }

        using (Transaction tx = new Transaction(doc, "Change Family and Type for Multiple Elements"))
        {
            tx.Start();
            foreach (var idStr in idStrings)
            {
                if (int.TryParse(idStr.Trim(), out int idInt))
                {
                    var element = doc.GetElement(new ElementId(idInt));
                    if (element != null)
                    {
                        try
                        {
                            element.ChangeTypeId(newType.Id);
                            changed.Add(idStr);
                        }
                        catch
                        {
                            failed.Add(idStr);
                        }
                    }
                    else
                    {
                        failed.Add(idStr);
                    }
                }
            }
            tx.Commit();
        }

        response["status"] = "partial_success";
        response["changed"] = string.Join(",", changed);
        response["failed"] = string.Join(",", failed);

        return response;
    }
}
