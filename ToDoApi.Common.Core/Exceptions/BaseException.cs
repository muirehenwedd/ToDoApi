using System;

namespace ToDoApi.Common.Core.Exceptions;

public class BaseException : Exception
{
    public BaseException(IExceptionScope errorScope, string message) : base(message)
    {
        ExceptionScope = errorScope;
    }

    public BaseException(IExceptionScope errorScope, string message, Exception innerException) : base(message,
        innerException)
    {
        ExceptionScope = errorScope;
    }

    public IExceptionScope ExceptionScope { get; }
}