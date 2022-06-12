using ToDoApi.Common.Auth.Attributes;
using ToDoApi.Common.Core.CQRS.Commands;

namespace ToDoApi.Services.Identity.Core.Commands;

[Authorize]
public class DeleteAccountCommand : ICommand
{
}