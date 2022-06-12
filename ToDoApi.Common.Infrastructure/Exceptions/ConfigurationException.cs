using System;
using ToDoApi.Common.Core.Exceptions;
using ToDoApi.Common.Infrastructure.ErrorHandling;

namespace ToDoApi.Common.Infrastructure.Exceptions;

public class ConfigurationException : BaseException
{
    public ConfigurationException(string message) : base(ErrorScope.Global, message)
    {
    }

    public ConfigurationException(string message, Exception innerException) : base(ErrorScope.Global, message,
        innerException)
    {
    }
}