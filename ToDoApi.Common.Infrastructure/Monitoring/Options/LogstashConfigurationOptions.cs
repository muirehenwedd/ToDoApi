using Serilog.Events;
using ToDoApi.Common.Infrastructure.Vault;
using ToDoApi.Common.Infrastructure.Vault.Attributes;

namespace ToDoApi.Common.Infrastructure.Monitoring.Options;

[VaultSecret("logstash")]
internal class LogstashConfigurationOptions
{
    public string Host { get; set; }
    public int Port { get; set; }
    public LogEventLevel MinimumLogLevel { get; set; }
}