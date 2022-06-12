using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog.Core;

namespace ToDoApi.Common.Infrastructure.Monitoring.Internals;

public class SerilogLogger : ILogger, IDisposable
{
    private readonly Logger _logger;
    private readonly JsonSerializerSettings _serializerSettings;

    public SerilogLogger(ISerilogConfigurationProvider serilogConfigurationProvider)
    {
        _logger = serilogConfigurationProvider.Configuration.CreateLogger();
        _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }

    public void Dispose()
    {
        _logger.Dispose();
    }

    public void Verbose(object message)
    {
        _logger
            .Verbose(Serialize(message));
    }

    public void Debug(object message, Exception exception)
    {
        _logger
            .Debug(exception, Serialize(message));
    }

    public void Information(object message)
    {
        _logger
            .Information(Serialize(message));
    }

    public void Warning(object message)
    {
        _logger
            .Warning(Serialize(message));
    }

    public void Error(object message, Exception exception)
    {
        _logger
            .Error(exception, Serialize(message));
    }

    public void Fatal(object message, Exception exception)
    {
        _logger
            .Fatal(exception, Serialize(message));
    }

    private string Serialize(object message)
    {
        return JsonConvert.SerializeObject(message, _serializerSettings);
    }
}