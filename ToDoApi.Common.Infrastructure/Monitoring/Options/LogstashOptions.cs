using ToDoApi.Common.Infrastructure.Attributes;

namespace ToDoApi.Common.Infrastructure.Monitoring.Options;

[ConfigOptions("Logstash")]
public sealed class LogstashOptions
{
    public bool UseLogstash { get; set; }
    public bool ManualHost { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
}