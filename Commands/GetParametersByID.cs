using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IoB_revitMCP
{
    public class GetParametersByIdCommand : ICommand
    {
        public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
        {
            var response = new Dictionary<string, object>();

            try
            {
                if (!input.TryGetValue("element_ids", out string elementIdsStr) || string.IsNullOrWhiteSpace(elementIdsStr))
                {
                    response["status"] = "error";
                    response["message"] = "Missing element_ids parameter.";
                    return response;
                }

                var doc = app?.ActiveUIDocument?.Document;
                if (doc == null)
                {
                    response["status"] = "error";
                    response["message"] = "No active Revit document.";
                    return response;
                }

                var result = new Dictionary<string, object>();
                var idStrings = elementIdsStr.Split(',');

                foreach (var idStr in idStrings)
                {
                    if (!int.TryParse(idStr.Trim(), out int elementIdInt)) continue;

                    var element = doc.GetElement(new ElementId(elementIdInt));
                    if (element == null)
                    {
                        result[$"{elementIdInt}"] = "Element not found.";
                        continue;
                    }

                    var paramData = new Dictionary<string, object>();
                    foreach (Parameter param in element.Parameters)
                    {
                        if (param == null) continue;

                        string name = param.Definition?.Name;
                        string storage = param.StorageType.ToString();
                        object value = null;

                        switch (param.StorageType)
                        {
                            case StorageType.String:
                                value = param.AsString();
                                break;
                            case StorageType.Double:
                                value = param.AsDouble();
                                break;
                            case StorageType.Integer:
                                value = param.AsInteger();
                                break;
                            case StorageType.ElementId:
                                value = param.AsElementId().IntegerValue;
                                break;
                            case StorageType.None:
                                value = null;
                                break;
                        }

                        if (!string.IsNullOrEmpty(name))
                        {
                            paramData[name] = new { value, storage };
                        }
                    }

                    result[$"{elementIdInt}"] = paramData;
                }

                response["status"] = "success";
                response["parameters"] = result;
                return response;
            }
            catch (Exception ex)
            {
                response["status"] = "error";
                response["message"] = ex.Message;
                return response;
            }
        }
    }
}
