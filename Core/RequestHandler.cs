// RequestHandler.cs - Updated to support ExecutePlan
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Text;
using System;
using IoB_revitMCP;

public class RequestHandler : IExternalEventHandler
{
    private readonly ConcurrentQueue<(string body, HttpListenerContext ctx)> _requests
        = new ConcurrentQueue<(string body, HttpListenerContext ctx)>();

    public static readonly Dictionary<string, ICommand> CommandMap = new Dictionary<string, ICommand>
    {
        { "CreateSheet" , new CreateSheetCommand() },
        { "PlaceViewsOnSheet", new PlaceViewsOnSheetCommand() },
        { "NewSharedParameter", new NewSharedParameterCommand() },
        { "ModifyElements", new ModifyElementsCommand() },
        { "GetElementParameters", new GetElementParametersCommand() },
        { "FilterByParameter", new FilterByParameterCommand() },
        { "ListElementsByCategory", new ListElementsCommand() },
        { "ExecutePlan", new PlanExecutorCommand() },
        { "AddViewFilter", new AddViewFilterCommand()},
        { "GetFamilyAndTypes", new GetFamiliesAndTypesCommand()},
        { "GetModelContext", new GetModelContextCommand()},
        { "ExportToJson" , new ExportToJsonCommand() }
    };

    public void SetRequest(string body, HttpListenerContext context)
    {
        _requests.Enqueue((body, context));
    }

    public void Execute(UIApplication app)
    {
        while (_requests.TryDequeue(out var req))
        {
            var requestBody = req.body;
            var context = req.ctx;

            var doc = app.ActiveUIDocument?.Document;
            var response = new Dictionary<string, object>();

        try
        {
            var request = JsonConvert.DeserializeObject<Dictionary<string, string>>(requestBody);

            if (request.ContainsKey("action") && CommandMap.ContainsKey(request["action"]))
            {
                response = CommandMap[request["action"].ToString()].Execute(app, request);
            }
            else if (request.ContainsKey("action") && request["action"] == "RunPython")
            {
                ScriptEngine engine = Python.CreateEngine();
                ScriptScope scope = engine.CreateScope();

                scope.SetVariable("doc", doc);
                scope.SetVariable("uidoc", app.ActiveUIDocument);
                scope.SetVariable("FilteredElementCollector", typeof(FilteredElementCollector));
                scope.SetVariable("Wall", typeof(Wall));
                scope.SetVariable("BuiltInCategory", typeof(BuiltInCategory));
                scope.SetVariable("ShowTaskDialog", new Action<string, string>(UiHelpers.ShowTaskDialog));
                scope.SetVariable("get_elements", new Func<Document, string, List<Element>>(RevitHelpers.GetElementsByCategory));
                scope.SetVariable("set_parameter", new Action<Element, string, string>(RevitHelpers.SetParameter));

                string script = request.ContainsKey("script") ? request["script"] : "print('No script provided')";

                var output = new StringWriter();
                engine.Runtime.IO.SetOutput(new MemoryStream(), output);

                engine.Execute(script, scope);
                response["status"] = "executed";
                response["output"] = output.ToString();
            }
            else
            {
                response["status"] = "unknown_action";
            }
        }
        catch (Exception ex)
        {
            response["status"] = "error";
            response["message"] = ex.Message;
        }

        string json = JsonConvert.SerializeObject(response);
        byte[] buffer = Encoding.UTF8.GetBytes(json);
            context.Response.ContentType = "application/json";
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.Close();
        }
    }

    public string GetName() => "MCP Request Handler";
}
