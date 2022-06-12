using System;
using System.Net;
using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Common.Infrastructure.Attributes;
using ToDoApi.Common.Infrastructure.CQRS.Commands;

namespace ToDoApi.Services.Identity.Core.Commands;

[CommandStatusMapper]
public class IdentityCommandStatusMapper : ICommandStatusMapper
{
    public HttpStatusCode Map(ICommand command)
    {
        return command switch
        {
            RegistrationCommand => HttpStatusCode.Created,
            SignInCommand => HttpStatusCode.OK,
            SignOutCommand => HttpStatusCode.OK,
            DeleteAccountCommand => HttpStatusCode.OK,
            _ => throw new ArgumentOutOfRangeException(nameof(command), command, null)
        };
    }
}