using System;
using ToDoApi.Common.Core.Exceptions;
using ToDoApi.Common.Infrastructure.ErrorHandling;

namespace ToDoApi.Services.Identity.Infrastructure.Exceptions;

public class UnauthorizedException : BaseException
{
    public UnauthorizedException() : base(ErrorScope.Identity, "Request is unauthorized.")
    {
    }

    public UnauthorizedException(Exception innerException) : base(ErrorScope.Identity, "Request is unauthorized.",
        innerException)
    {
    }
}