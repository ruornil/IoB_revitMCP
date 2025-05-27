// McpServer.cs - Minimal HTTP listener (localhost only)
using System;
using System.Net;
using System.Text;
using System.Threading;
using Autodesk.Revit.UI;

public static class McpServer
{
    private static HttpListener _listener;
    private static Thread _thread;
    private static ExternalEvent _externalEvent;
    private static RequestHandler _handler;

    public static void Start()
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add("http://localhost:5005/mcp/");
        _listener.Start();

        _handler = new RequestHandler();
        _externalEvent = ExternalEvent.Create(_handler);

        _thread = new Thread(ListenLoop);
        _thread.Start();
    }

    public static void Stop()
    {
        _listener?.Stop();
        _thread?.Join();
    }

    private static void ListenLoop()
    {
        while (_listener.IsListening)
        {
            try
            {
                var context = _listener.GetContext();
                var reader = new System.IO.StreamReader(context.Request.InputStream);
                string requestBody = reader.ReadToEnd();
                reader.Close();

                _handler.SetRequest(requestBody, context);
                _externalEvent.Raise();
            }
            catch { }
        }
    }
}