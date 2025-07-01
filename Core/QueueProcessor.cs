using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Newtonsoft.Json;

/// <summary>
/// Background worker that executes queued plans from the mcp_queue table.
/// </summary>
public static class QueueProcessor
{
    private static CancellationTokenSource _cts;
    private static Task _task;
    private static UIApplication _uiApp;
    private static bool _started;

    public static void Start(UIApplication app)
    {
        if (_started) return;
        _uiApp = app;
        _cts = new CancellationTokenSource();
        _task = Task.Run(() => ProcessLoop(_cts.Token));
        _started = true;
    }

    public static void Stop()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _task?.Wait(1000);
            _cts.Dispose();
        }
        _started = false;
    }

    private static void ProcessLoop(CancellationToken token)
    {
        string conn = System.Configuration.ConfigurationManager.ConnectionStrings["mcp"]?.ConnectionString;
        if (string.IsNullOrEmpty(conn)) return;
        var db = new PostgresDb(conn);
        var planCmd = new PlanExecutorCommand();
        while (!token.IsCancellationRequested)
        {
            try
            {
                var (id, plan) = db.DequeuePlan();
                if (id == 0)
                {
                    Task.Delay(1000, token).Wait(token);
                    continue;
                }

                var input = new Dictionary<string, string>
                {
                    {"steps", plan }
                };
                var result = planCmd.Execute(_uiApp, input);
                string jsonResult = JsonConvert.SerializeObject(result);
                db.SetJobResult(id, "done", jsonResult);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch
            {
                // swallow and continue
            }
        }
    }
}
