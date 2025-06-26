// GetElementParametersCommand.cs - Retrieves parameters for elements by ID or selection
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;


public class GetElementParametersCommand : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var response = new Dictionary<string, object>();
        var uidoc = app.ActiveUIDocument;
        var doc = uidoc?.Document;

        if (doc == null)
        {
            response["status"] = "error";
            response["message"] = "No active document.";
            return response;
        }
        string conn = DbConfigHelper.GetConnectionString(input);
        PostgresDb db = null;
        if (!string.IsNullOrEmpty(conn))
            db = new PostgresDb(conn);

        var ids = new List<ElementId>();
        if (input.TryGetValue("element_ids", out var idStr) && !string.IsNullOrWhiteSpace(idStr))
        {
            foreach (var part in idStr.Split(','))
            {
                if (int.TryParse(part.Trim(), out var intId))
                    ids.Add(new ElementId(intId));
            }
        }
        else
        {
            ids.AddRange(uidoc.Selection.GetElementIds());
        }

        if (ids.Count == 0)
        {
            response["status"] = "error";
            response["message"] = "No elements specified or selected.";
            return response;
        }

        var result = new Dictionary<string, object>();
        var paramNames = new HashSet<string>();
        DateTime now = DateTime.UtcNow;
        foreach (var id in ids)
        {
            var element = doc.GetElement(id);
            if (element == null)
            {
                result[id.IntegerValue.ToString()] = "Element not found.";
                continue;
            }

            string typeName = string.Empty;
            var et = doc.GetElement(element.GetTypeId()) as ElementType;
            if (et != null) typeName = et.Name;

            string levelName = string.Empty;
            if (element.LevelId != ElementId.InvalidElementId)
            {
                var lvl = doc.GetElement(element.LevelId);
                if (lvl != null) levelName = lvl.Name;
            }

            if (db != null)
            {
                db.UpsertElement(
                    element.Id.IntegerValue,
                    ParseGuid(element.UniqueId),
                    element.Name,
                    element.Category?.Name ?? string.Empty,
                    typeName,
                    levelName,
                    doc.PathName,
                    now);
            }

            var paramData = new Dictionary<string, object>();
            foreach (Parameter param in element.Parameters)
            {
                if (param == null) continue;
                string name = param.Definition?.Name;
                if (string.IsNullOrEmpty(name)) continue;
                paramNames.Add(name);
                string storage = param.StorageType.ToString();
                object value = null;
                string valueStr = null;
                switch (param.StorageType)
                {
                    case StorageType.String:
                        value = param.AsString();
                        valueStr = param.AsString();
                        break;
                    case StorageType.Double:
                        value = param.AsDouble();
                        valueStr = param.AsDouble().ToString();
                        break;
                    case StorageType.Integer:
                        value = param.AsInteger();
                        valueStr = param.AsInteger().ToString();
                        break;
                    case StorageType.ElementId:
                        value = param.AsElementId().IntegerValue;
                        valueStr = param.AsElementId().IntegerValue.ToString();
                        break;
                }
                bool isType = param.Element != null && param.Element.Id != element.Id;
                var pInfo = new Dictionary<string, object>();
                pInfo["value"] = value;
                pInfo["storage"] = storage;
                pInfo["is_type"] = isType;
                paramData[name] = pInfo;

                if (db != null)
                {
                    db.UpsertParameter(element.Id.IntegerValue, name, valueStr, isType, null);
                }
            }
            result[id.IntegerValue.ToString()] = paramData;
        }

        response["status"] = "success";
        response["parameters"] = result;
        response["parameter_names"] = paramNames.ToList();
        return response;
    }

    private static Guid ParseGuid(string uid)
    {
        if (string.IsNullOrEmpty(uid)) return Guid.Empty;
        if (uid.Length >= 36)
        {
            Guid g;
            if (Guid.TryParse(uid.Substring(0, 36), out g)) return g;
        }
        return Guid.Empty;
    }
}
