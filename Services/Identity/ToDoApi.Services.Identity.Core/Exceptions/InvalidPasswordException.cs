using ToDoApi.Common.Core.Exceptions;

namespace ToDoApi.Services.Identity.Core.Exceptions;

public class InvalidPasswordException : BaseException
{
    public InvalidPasswordException(string login) : base(IdentityExceptionScope.SignIn,
        $"Unable to create user session: invalid password was provided for user with login '{login}'")
    {
    }
}