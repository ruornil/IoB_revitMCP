// NewSharedParameter.cs - Creates and binds shared parameters to categories in Revit
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class NewSharedParameterCommand : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var response = new Dictionary<string, object>();
        var doc = app.ActiveUIDocument?.Document;

        if (doc == null)
        {
            response["status"] = "error";
            response["message"] = "No active document.";
            return response;
        }

        try
        {
            if (!input.TryGetValue("parameter_name", out var paramName) ||
                !input.TryGetValue("parameter_group", out var paramGroupName) ||
                !input.TryGetValue("categories", out var categoriesStr) ||
                !input.TryGetValue("binding_type", out var bindingType))
            {
                response["status"] = "error";
                response["message"] = "Missing required inputs: parameter_name, parameter_group, categories, binding_type.";
                return response;
            }

            // Determine the shared parameter file
            var sharedParamsFilename = doc.Application.SharedParametersFilename;
            if (string.IsNullOrEmpty(sharedParamsFilename) || !File.Exists(sharedParamsFilename))
            {
                response["status"] = "error";
                response["message"] = "Shared parameter file is not set or does not exist. Please configure it via Application.SharedParametersFilename.";
                return response;
            }

            var defFile = doc.Application.OpenSharedParameterFile();
            if (defFile == null)
            {
                response["status"] = "error";
                response["message"] = "Unable to open shared parameter file.";
                return response;
            }

            // Default to text-type parameter creation (ParameterType is internal)
            ExternalDefinitionCreationOptions options = new ExternalDefinitionCreationOptions(paramName, SpecTypeId.String.Text);

            Definition definition = null;
            DefinitionGroup groupDef = defFile.Groups.Cast<DefinitionGroup>().FirstOrDefault(g => g.Definitions.Cast<Definition>().Any(d => d.Name == paramName));
            if (groupDef != null)
            {
                definition = groupDef.Definitions.get_Item(paramName);
            }
            if (definition == null)
            {
                if (groupDef == null) groupDef = defFile.Groups.Create("MCP");
                definition = groupDef.Definitions.Create(options);
            }

            var paramGroup = Enum.TryParse<BuiltInParameterGroup>(paramGroupName, out var group) ? group : BuiltInParameterGroup.PG_DATA;

            // Target categories
            var categoryNames = categoriesStr.Split(',').Select(s => s.Trim());
            var catSet = new CategorySet();

            foreach (var catName in categoryNames)
            {
                if (Enum.IsDefined(typeof(BuiltInCategory), "OST_" + catName))
                {
                    var bic = (BuiltInCategory)Enum.Parse(typeof(BuiltInCategory), "OST_" + catName);
                    {
                        var cat = doc.Settings.Categories.get_Item(bic);
                        if (cat != null) catSet.Insert(cat);
                    }
                }

                using (var tx = new Transaction(doc, "Bind Shared Parameter"))
                {
                    tx.Start();

                    ElementBinding binding;
                    if (doc.ParameterBindings.Contains(definition))
                    {
                        binding = doc.ParameterBindings.get_Item(definition) as ElementBinding;
                    }
                    else
                    {
                        if (bindingType.Equals("Type", StringComparison.OrdinalIgnoreCase))
                            binding = doc.Application.Create.NewTypeBinding(catSet);
                        else
                            binding = doc.Application.Create.NewInstanceBinding(catSet);
                    }

                    doc.ParameterBindings.Remove(definition);
                    doc.ParameterBindings.Insert(definition, binding, paramGroup);
                    tx.Commit();
                }

                response["status"] = "success";
                response["parameter"] = paramName;
                response["categories"] = categoryNames.ToList();
            }
        }
        catch (Exception ex)
        {
            response["status"] = "error";
            response["message"] = ex.Message;
        }

        return response;
    }
}
