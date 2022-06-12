using ToDoApi.Common.Core.Exceptions;

namespace ToDoApi.Services.Identity.Core.Exceptions;

public class SessionAlreadyEndedException:BaseException
{
    public SessionAlreadyEndedException() : base(IdentityExceptionScope.SignIn,
        $"Session already ended.")
    {
    }
}