using ToDoApi.Common.Core.Exceptions;

namespace ToDoApi.Services.Identity.Core.Exceptions;

public class UserIsNotFoundException : BaseException
{
    public UserIsNotFoundException() : base(IdentityExceptionScope.SignUp,
        "User is not found.")
    {
    }
}