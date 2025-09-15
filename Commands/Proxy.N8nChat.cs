using Autodesk.Revit.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

public class ProxyN8nChatCommand : ICommand
{
    public Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input)
    {
        var response = new Dictionary<string, object>();

        try
        {
            if (!input.TryGetValue("webhook", out string webhook) || string.IsNullOrWhiteSpace(webhook))
            {
                response["status"] = "error";
                response["message"] = "Missing 'webhook'";
                return response;
            }
            // Accept either 'chatInput' or legacy 'message'
            string message;
            if (input.ContainsKey("chatInput")) message = input["chatInput"]; else input.TryGetValue("message", out message);
            if (string.IsNullOrWhiteSpace(message))
            {
                response["status"] = "error";
                response["message"] = "Missing 'chatInput' (or 'message')";
                return response;
            }
            input.TryGetValue("session", out string session);

            // Restrict proxy to localhost for safety
            if (!webhook.StartsWith("http://localhost", StringComparison.OrdinalIgnoreCase)
                && !webhook.StartsWith("https://localhost", StringComparison.OrdinalIgnoreCase)
                && !webhook.StartsWith("http://127.0.0.1", StringComparison.OrdinalIgnoreCase)
                && !webhook.StartsWith("https://127.0.0.1", StringComparison.OrdinalIgnoreCase))
            {
                response["status"] = "error";
                response["message"] = "Proxy blocked: only localhost webhooks allowed.";
                return response;
            }

            var payload = new Dictionary<string, object>
            {
                { "chatInput", message },
                { "sessionId", string.IsNullOrEmpty(session) ? Guid.NewGuid().ToString("N") : session }
            };

            using (var http = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var res = http.PostAsync(webhook, content).GetAwaiter().GetResult();
                var body = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                response["status"] = res.IsSuccessStatusCode ? "success" : "error";
                response["code"] = (int)res.StatusCode;
                response["raw"] = body;

                try
                {
                    var parsed = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);
                    if (parsed != null)
                    {
                        response["reply"] =
                            (parsed.ContainsKey("reply") ? parsed["reply"] : null) ??
                            (parsed.ContainsKey("response") ? parsed["response"] : null) ??
                            (parsed.ContainsKey("text") ? parsed["text"] : null);
                        response["json"] = parsed;
                    }
                }
                catch { /* non-JSON body */ }
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
