using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Serilog;
using ToDoApi.Common.Infrastructure.Monitoring.Formatter;
using ToDoApi.Common.Infrastructure.Monitoring.Options;
using ToDoApi.Common.Infrastructure.Monitoring.Sink;
using ToDoApi.Common.Infrastructure.Security.Options;
using ToDoApi.Common.Infrastructure.Vault;

namespace ToDoApi.Common.Infrastructure.Monitoring.Internals;

internal class SerilogConfigurationProvider : ISerilogConfigurationProvider
{
    private readonly IKeyValueSecretProvider _keyValueSecretProvider;
    private readonly LogstashOptions _logstashOptions;
    private readonly SslOptions _sslOptions;

    private LoggerConfiguration _loggerConfiguration;

    public SerilogConfigurationProvider(
        SslOptions sslOptions,
        LogstashOptions logstashOptions,
        IKeyValueSecretProvider keyValueSecretProvider
    )
    {
        _sslOptions = sslOptions;
        _logstashOptions = logstashOptions;
        _keyValueSecretProvider = keyValueSecretProvider;
    }

    public LoggerConfiguration Configuration =>
        _loggerConfiguration ??= BuildConfiguration().GetAwaiter().GetResult();

    private async Task<LoggerConfiguration> BuildConfiguration()
    {
        var options = await _keyValueSecretProvider.GetAsync<LogstashConfigurationOptions>();
        var host = _logstashOptions.ManualHost ? _logstashOptions.Host : options.Host;
        var port = _logstashOptions.ManualHost ? _logstashOptions.Port : options.Port;

        var clientCertificate = new X509Certificate2(_sslOptions.PfxCertificate, _sslOptions.PfxPassword);
        var rootCertificates = new X509Certificate2(_sslOptions.RootAuthority);

        var loggerConfiguration = new LoggerConfiguration()
            .Enrich.FromLogContext();

        loggerConfiguration.WriteTo.Console(new ConsoleLogFormatter(), options.MinimumLogLevel);

        if (_logstashOptions.UseLogstash)
            loggerConfiguration.WriteTo.LogstashTcpSink(
                host,
                port,
                clientCertificate,
                rootCertificates,
                options.MinimumLogLevel);

        return loggerConfiguration;
    }
}