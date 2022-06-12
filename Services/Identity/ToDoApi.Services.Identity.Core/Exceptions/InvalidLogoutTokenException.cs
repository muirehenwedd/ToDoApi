using ToDoApi.Common.Core.Exceptions;

namespace ToDoApi.Services.Identity.Core.Exceptions;

public class InvalidLogoutTokenException : BaseException
{
    public InvalidLogoutTokenException() : base(IdentityExceptionScope.SignIn,
        "Unable to logout: invalid token was provided.")
    {
    }
}