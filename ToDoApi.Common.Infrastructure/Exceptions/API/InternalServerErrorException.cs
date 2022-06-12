using System;
using ToDoApi.Common.Core.Exceptions;
using ToDoApi.Common.Infrastructure.ErrorHandling;

namespace ToDoApi.Common.Infrastructure.Exceptions.API;

public class InternalServerErrorException : BaseException
{
    public InternalServerErrorException(string message) : base(ErrorScope.Global, message)
    {
    }

    public InternalServerErrorException(string message, Exception innerException) : base(ErrorScope.Global, message,
        innerException)
    {
    }
}