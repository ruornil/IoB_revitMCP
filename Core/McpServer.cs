// McpServer.cs - Minimal HTTP listener (async)
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;

public static class McpServer
{
    private static HttpListener _listener;
    private static Task _listenTask;
    private static ExternalEvent _externalEvent;
    private static RequestHandler _handler;

    public static void Start()
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add("http://localhost:5005/mcp/");
        _listener.Start();

        _handler = new RequestHandler();
        _externalEvent = ExternalEvent.Create(_handler);

        _listenTask = ListenAsync();
    }

    public static void Stop()
    {
        _listener?.Stop();
        _listenTask?.Wait();
    }

    private static async Task ListenAsync()
    {
        while (_listener.IsListening)
        {
            HttpListenerContext context = null;
            try
            {
                context = await _listener.GetContextAsync();
            }
            catch (HttpListenerException)
            {
                break;
            }
            catch (Exception ex)
            {
                LogError($"Accept error: {ex}");
                continue;
            }

            _ = ProcessRequestAsync(context);
        }
    }

    private static async Task ProcessRequestAsync(HttpListenerContext context)
    {
        try
        {
            using var reader = new System.IO.StreamReader(context.Request.InputStream);
            string requestBody = await reader.ReadToEndAsync();

            _handler.SetRequest(requestBody, context);
            _externalEvent.Raise();
        }
        catch (Exception ex)
        {
            LogError($"Process error: {ex}");
            try
            {
                context.Response.StatusCode = 500;
                byte[] buffer = Encoding.UTF8.GetBytes("{\"status\":\"error\"}");
                await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                context.Response.Close();
            }
            catch { }
        }
    }

    private static void LogError(string message)
    {
        try
        {
            System.IO.File.AppendAllText("mcp.log",
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {message}{Environment.NewLine}");
        }
        catch { }
    }
}
