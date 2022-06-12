using System.Security.Cryptography.X509Certificates;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.Network.Formatters;

namespace ToDoApi.Common.Infrastructure.Monitoring.Sink;

internal static class Extensions
{
    internal static LoggerConfiguration LogstashTcpSink(
        this LoggerSinkConfiguration configuration,
        string host,
        int port,
        X509Certificate2 clientCertificate,
        X509Certificate2 rootCertificate,
        LogEventLevel restrictedToMinimumLevel
    )
    {
        var sink = new LogstashTcpSink(host, port, clientCertificate, rootCertificate, new LogstashJsonFormatter());
        return configuration.Sink(sink, restrictedToMinimumLevel);
    }
}