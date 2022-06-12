using System;
using System.Net;
using ToDoApi.Common.Infrastructure.Attributes;
using ToDoApi.Common.Infrastructure.ErrorHandling;
using ToDoApi.Common.Infrastructure.Exceptions;
using ToDoApi.Common.Infrastructure.Exceptions.API;
using ToDoApi.Services.Identity.Infrastructure.Exceptions;

namespace ToDoApi.Bootstrapper.ErrorHandling;

[ExceptionMapper]
public class GlobalExceptionMapper : IExceptionMapper
{
    public (int, string, HttpStatusCode) Map(Exception exception)
    {
        return exception switch
        {
            ConfigurationException => (101, exception.Message, HttpStatusCode.InternalServerError),
            CommandHandlerNotRegisteredException => (101, exception.Message, HttpStatusCode.InternalServerError),
            QueryHandlerNotRegisteredException => (101, exception.Message, HttpStatusCode.InternalServerError),
            BadRequestException => (103, exception.Message, HttpStatusCode.BadRequest),
            ForbiddenException => (104, exception.Message, HttpStatusCode.Forbidden),
            InternalServerErrorException => (105, exception.Message, HttpStatusCode.InternalServerError),
            UnauthorizedException => (107, "Client is unauthorized: invalid access token.",
                HttpStatusCode.Unauthorized),
            _ => throw new ArgumentOutOfRangeException(nameof(exception), exception, null)
        };
    }
}