using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Serilog.Events;
using Serilog.Formatting;

namespace ToDoApi.Common.Infrastructure.Monitoring.Formatter;

internal class ConsoleLogFormatter : ITextFormatter
{
    public void Format(LogEvent logEvent, TextWriter output)
    {
        output.Write($"[{logEvent.Level}] – {logEvent.Timestamp}");

        if (logEvent.Exception != null)
            output.WriteLine($" – {logEvent.Exception.Message}");
        else
            output.WriteLine("");

        foreach (var (key, value) in logEvent.Properties) output.WriteLine($"{key}: {value.ToString()}");

        var message = logEvent.MessageTemplate.Render(logEvent.Properties);
        var messageObject = JsonSerializer.Deserialize<IDictionary<string, object>>(message);

        if (messageObject != null)
        {
            messageObject.Remove("stackTrace");
            messageObject.Remove("exception");
            messageObject.Remove("errorMessage");

            var jsonMessage = JsonSerializer.Serialize(messageObject, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            output.WriteLine(jsonMessage);
        }

        if (logEvent.Exception != null)
        {
            output.WriteLine($"Exception: {logEvent.Exception.GetType()}");
            output.WriteLine($"Message: {logEvent.Exception.Message}");
            output.WriteLine("Stack Trace:");
            output.WriteLine(logEvent.Exception.ToString());
        }
    }
}