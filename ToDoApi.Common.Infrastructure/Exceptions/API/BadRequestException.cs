using System;
using ToDoApi.Common.Core.Exceptions;
using ToDoApi.Common.Infrastructure.ErrorHandling;

namespace ToDoApi.Common.Infrastructure.Exceptions.API;

public class BadRequestException : BaseException
{
    public BadRequestException(string message) : base(ErrorScope.RequestHandling, message)
    {
    }

    public BadRequestException(
        Type payloadType,
        object validationResult
    ) : base(ErrorScope.RequestHandling, $"Validation failed for request body of type '{payloadType}'.")
    {
        ValidationResult = validationResult;
    }

    public BadRequestException(string message, Exception innerException) : base(ErrorScope.RequestHandling, message,
        innerException)
    {
    }

    public object? ValidationResult { get; }
}