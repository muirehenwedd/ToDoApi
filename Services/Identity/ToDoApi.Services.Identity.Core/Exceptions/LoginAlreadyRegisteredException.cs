using ToDoApi.Common.Core.Exceptions;

namespace ToDoApi.Services.Identity.Core.Exceptions;

public class LoginAlreadyRegisteredException : BaseException
{
    public LoginAlreadyRegisteredException(string login) : base(IdentityExceptionScope.SignUp,
        $"Login '{login}' is already exists.")
    {
    }
}