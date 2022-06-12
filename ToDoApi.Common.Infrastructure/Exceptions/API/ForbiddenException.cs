using ToDoApi.Common.Core.Exceptions;
using ToDoApi.Common.Infrastructure.ErrorHandling;

namespace ToDoApi.Common.Infrastructure.Exceptions.API;

public class ForbiddenException : BaseException
{
    public ForbiddenException() : base(ErrorScope.Global,
        "Client is not authorized to access the particular resource.")
    {
    }
}