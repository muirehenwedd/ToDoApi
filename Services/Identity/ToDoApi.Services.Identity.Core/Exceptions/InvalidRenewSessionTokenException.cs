using ToDoApi.Common.Core.Exceptions;

namespace ToDoApi.Services.Identity.Core.Exceptions;

public class InvalidRenewSessionTokenException : BaseException
{
    public InvalidRenewSessionTokenException() : base(IdentityExceptionScope.SignIn,
        "Unable to renew user session: invalid token was provided. Sign in again")
    {
    }
}