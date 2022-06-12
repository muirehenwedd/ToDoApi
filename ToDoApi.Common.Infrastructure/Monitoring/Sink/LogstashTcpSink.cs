using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace ToDoApi.Common.Infrastructure.Monitoring.Sink;

internal class LogstashTcpSink : ILogEventSink, IDisposable
{
    private readonly ITextFormatter _formatter;
    private readonly SecureTcpSocketWriter _socketWriter;

    public LogstashTcpSink(
        string host,
        int port,
        X509Certificate2 clientCertificate,
        X509Certificate2 rootCertificate,
        ITextFormatter formatter
    )
    {
        _formatter = formatter;
        _socketWriter = new SecureTcpSocketWriter(host, port, clientCertificate, rootCertificate);
    }

    public void Dispose()
    {
        _socketWriter.Dispose();
    }

    public void Emit(LogEvent logEvent)
    {
        var builder = new StringBuilder();

        using var stringWriter = new StringWriter(builder);

        _formatter.Format(logEvent, stringWriter);
        builder.Replace("RenderedMessage", "message");

        _socketWriter.Enqueue(builder.ToString());
    }
}