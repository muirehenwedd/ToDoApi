using System;
using System.Net;
using ToDoApi.Common.Infrastructure.Attributes;
using ToDoApi.Common.Infrastructure.ErrorHandling;
using ToDoApi.Services.Identity.Core.Exceptions;

namespace ToDoApi.Services.Identity.Infrastructure.Exceptions;

[ExceptionMapper]
public class IdentityExceptionMapper : IExceptionMapper
{
    public (int, string, HttpStatusCode) Map(Exception exception)
    {
        return exception switch
        {
            LoginAlreadyRegisteredException => (201, exception.Message, HttpStatusCode.Conflict),
            UserIsNotFoundException => (202, exception.Message, HttpStatusCode.NotFound),
            InvalidPasswordException => (203, exception.Message, HttpStatusCode.Forbidden),
            RevokedTokenException => (204, exception.Message, HttpStatusCode.Forbidden),
            InvalidRenewSessionTokenException => (205, exception.Message, HttpStatusCode.NotFound),
            SessionAlreadyEndedException => (206, exception.Message, HttpStatusCode.Forbidden),
            InvalidLogoutTokenException => (207, exception.Message, HttpStatusCode.NotFound),
            
            

            _ => throw new ArgumentOutOfRangeException(nameof(exception), exception, null)
        };
    }
}