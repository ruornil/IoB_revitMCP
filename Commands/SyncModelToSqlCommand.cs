using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Newtonsoft.Json;
using Npgsql;

public class SyncModelToSqlCommand : ICommand
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

        string conn = DbConfigHelper.GetConnectionString(input);
        if (string.IsNullOrEmpty(conn))
        {
            response["status"] = "error";
            response["message"] = "No connection string found. " + DbConfigHelper.GetHelpMessage();
            return response;
        }

        // Run connection test before instantiating PostgresDb
        try
        {
            using (var testConn = new Npgsql.NpgsqlConnection(conn))
            {
                testConn.Open(); // This will throw if Npgsql or dependencies are broken
            }
        }
        catch (Exception ex)
        {
            System.IO.File.WriteAllText("C:\\Temp\\pg-debug.txt", ex.ToString());
            response["status"] = "error";
            response["message"] = "Connection test failed. See pg-debug.txt for details.";
            return response;
        }

        var db = new BatchedPostgresDb(conn);

        // If async flag provided, enqueue a plan and return job id
        if (input.TryGetValue("async", out var asyncVal) && asyncVal == "true")
        {
            var paramCopy = new Dictionary<string, string>(input);
            paramCopy.Remove("action");
            paramCopy.Remove("async");
            var step = new Dictionary<string, object>
            {
                { "action", "SyncModelToSql" },
                { "params", paramCopy }
            };
            string planJson = JsonConvert.SerializeObject(new[] { step });
            int jobId = db.EnqueuePlan(planJson);
            response["status"] = "queued";
            response["job_id"] = jobId;
            return response;
        }

        // Proceed as usual if the test passed
        
        DateTime lastSaved = System.IO.File.GetLastWriteTime(doc.PathName);
        if (db.GetModelLastSaved(doc.PathName) == lastSaved)
        {
            response["status"] = "up_to_date";
            db.CommitAll();
            return response;
        }

        // capture model info and parameters
        var projectInfo = doc.ProjectInformation;
        var info = new Dictionary<string, string>();
        foreach (Parameter p in projectInfo.Parameters)
        {
            if (p == null || string.IsNullOrEmpty(p.Definition?.Name)) continue;
            string val = string.Empty;
            switch (p.StorageType)
            {
                case StorageType.String:
                    val = p.AsString();
                    break;
                case StorageType.Integer:
                    val = p.AsInteger().ToString();
                    break;
                case StorageType.Double:
                    val = p.AsDouble().ToString();
                    break;
                case StorageType.ElementId:
                    val = p.AsElementId().IntegerValue.ToString();
                    break;
            }
            info[p.Definition.Name] = val;
        }

        var bindingMap = doc.ParameterBindings;
        var iterator = bindingMap.ForwardIterator();
        var parameters = new List<Dictionary<string, object>>();
        iterator.Reset();
        while (iterator.MoveNext())
        {
            Definition definition = iterator.Key;
            ElementBinding binding = iterator.Current as ElementBinding;
            if (definition == null || binding == null) continue;
            var paramData = new Dictionary<string, object>();
            paramData["name"] = definition.Name;
            paramData["parameter_type"] = definition.ParameterGroup.ToString();
            paramData["unit_type_id"] = definition.GetDataType()?.TypeId?.ToString() ?? string.Empty;
            paramData["binding_type"] = binding is InstanceBinding ? "Instance" : "Type";
            var categories = new List<string>();
            foreach (Category cat in binding.Categories)
            {
                if (cat != null) categories.Add(cat.Name);
            }
            paramData["categories"] = categories;
            parameters.Add(paramData);
        }

        string jsonInfo = JsonSerializer.Serialize(info);
        string jsonParams = JsonSerializer.Serialize(parameters);
        db.UpsertModelInfo(doc.PathName, doc.Title, ParseGuid(doc.ProjectInformation.UniqueId), lastSaved, jsonInfo, jsonParams);

        // gather element types once and store in DB
        var typeCollector = new FilteredElementCollector(doc).WhereElementIsElementType();
        var typeMap = new Dictionary<ElementId, string>();
        foreach (ElementType type in typeCollector)
        {
            db.UpsertElementType(type.Id.IntegerValue, ParseGuid(type.UniqueId), type.FamilyName, type.Name, type.Category?.Name ?? string.Empty, doc.PathName, lastSaved);
            if (!typeMap.ContainsKey(type.Id))
                typeMap[type.Id] = type.Name;
        }

        var collector = new FilteredElementCollector(doc).WhereElementIsNotElementType();
        int count = 0;
        foreach (var element in collector)
        {
            string typeName = string.Empty;
            if (typeMap.TryGetValue(element.GetTypeId(), out string tname))
                typeName = tname;

            string levelName = string.Empty;
            if (element.LevelId != ElementId.InvalidElementId)
            {
                var lvl = doc.GetElement(element.LevelId);
                if (lvl != null) levelName = lvl.Name;
            }
            db.StageElement(element.Id.IntegerValue, ParseGuid(element.UniqueId), element.Name, element.Category?.Name ?? string.Empty, typeName, levelName, doc.PathName, lastSaved);
            count++;
        }
        // Optional: sync link instances metadata if requested
        bool syncLinks = input.TryGetValue("sync_links", out var syncLinksVal) && syncLinksVal.Equals("true", StringComparison.OrdinalIgnoreCase);
        if (syncLinks)
        {
            try
            {
                var instances = new FilteredElementCollector(doc)
                    .OfClass(typeof(RevitLinkInstance))
                    .Cast<RevitLinkInstance>();
                foreach (var inst in instances)
                {
                    string hostDocId = doc.PathName;
                    int instanceId = inst.Id.IntegerValue;

                    // Try to resolve link document id (path)
                    string linkDocId = string.Empty;
                    Document linkDoc = null;
                    try { linkDoc = inst.GetLinkDocument(); } catch { linkDoc = null; }
                    if (linkDoc != null && !string.IsNullOrWhiteSpace(linkDoc.PathName))
                        linkDocId = linkDoc.PathName;
                    else
                    {
                        // fallback to type path if available
                        try
                        {
                            var linkType = doc.GetElement(inst.GetTypeId()) as RevitLinkType;
                            var path = linkType?.PathName;
                            if (string.IsNullOrWhiteSpace(path))
                            {
                                var ext = linkType?.GetExternalFileReference();
                                var mp = ext?.GetPath();
                                if (mp != null)
                                    path = ModelPathUtils.ConvertModelPathToUserVisiblePath(mp);
                            }
                            linkDocId = path ?? string.Empty;
                        }
                        catch { linkDocId = string.Empty; }
                    }

                    double? ox = null, oy = null, oz = null;
                    double? bxx = null, bxy = null, bxz = null;
                    double? byx = null, byy = null, byz = null;
                    double? bzx = null, bzy = null, bzz = null;
                    double? rotz = null, atn = null;
                    try
                    {
                        Transform t = inst.GetTotalTransform();
                        ox = t.Origin.X; oy = t.Origin.Y; oz = t.Origin.Z;
                        bxx = t.BasisX.X; bxy = t.BasisX.Y; bxz = t.BasisX.Z;
                        byx = t.BasisY.X; byy = t.BasisY.Y; byz = t.BasisY.Z;
                        bzx = t.BasisZ.X; bzy = t.BasisZ.Y; bzz = t.BasisZ.Z;
                        rotz = Math.Atan2(t.BasisX.Y, t.BasisX.X);
                    }
                    catch { }
                    if (linkDoc != null)
                    {
                        try
                        {
                            var loc = linkDoc.ActiveProjectLocation;
                            var pp = loc?.GetProjectPosition(XYZ.Zero);
                            if (pp != null) atn = pp.Angle;
                        }
                        catch { }
                    }

                    // Write link instance row
                    var pg = new PostgresDb(conn);
                    pg.UpsertLinkInstance(hostDocId, instanceId, linkDocId,
                        ox, oy, oz,
                        bxx, bxy, bxz,
                        byx, byy, byz,
                        bzx, bzy, bzz,
                        rotz, atn, lastSaved);
                }
            }
            catch (Exception ex)
            {
                // Non-fatal: include error in response
                response["sync_links_error"] = ex.Message;
            }
        }
        db.CommitAll();
        response["status"] = "success";
        response["updated"] = count;
        response["model_name"] = doc.Title;
        response["project_info"] = info;
        response["project_parameters"] = parameters;
        response["guid"] = ParseGuid(doc.ProjectInformation.UniqueId).ToString();
        response["last_saved"] = lastSaved.ToString("yyyy-MM-ddTHH:mm:ss");
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
