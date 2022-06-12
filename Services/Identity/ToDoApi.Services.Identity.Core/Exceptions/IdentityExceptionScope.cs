using ToDoApi.Common.Core.Exceptions;

namespace ToDoApi.Services.Identity.Core.Exceptions;

public class IdentityExceptionScope : IExceptionScope
{
    public IdentityExceptionScope(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public static IdentityExceptionScope SignUp => new("sign-up");
    public static IdentityExceptionScope SignIn => new("sign-in");
}