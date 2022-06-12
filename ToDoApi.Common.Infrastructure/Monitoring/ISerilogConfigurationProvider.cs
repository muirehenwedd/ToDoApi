using Serilog;

namespace ToDoApi.Common.Infrastructure.Monitoring;

public interface ISerilogConfigurationProvider
{
    public LoggerConfiguration Configuration { get; }
}