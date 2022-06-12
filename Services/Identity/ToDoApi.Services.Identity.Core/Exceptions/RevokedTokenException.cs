using ToDoApi.Common.Core.Exceptions;

namespace ToDoApi.Services.Identity.Core.Exceptions;

public class RevokedTokenException : BaseException
{
    public RevokedTokenException() : base(IdentityExceptionScope.SignIn,
        "Token is revoked.")
    {
    }
}