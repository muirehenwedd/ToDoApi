using System;

namespace ToDoApi.Common.Infrastructure.Monitoring;

public interface ILogger
{
    void Verbose(object message);
    void Debug(object message, Exception exception);
    void Information(object message);
    void Warning(object message);
    void Error(object message, Exception exception);
    void Fatal(object message, Exception exception);
}