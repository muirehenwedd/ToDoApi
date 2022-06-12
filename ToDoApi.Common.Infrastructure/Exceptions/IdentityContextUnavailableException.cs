using ToDoApi.Common.Core.Exceptions;
using ToDoApi.Common.Infrastructure.ErrorHandling;

namespace ToDoApi.Common.Infrastructure.Exceptions;

public class IdentityContextUnavailableException : BaseException
{
    public IdentityContextUnavailableException() : base(ErrorScope.Identity,
        "Identity context is unavailable for this request")
    {
    }
}