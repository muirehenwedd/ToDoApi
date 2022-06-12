using ToDoApi.Common.Auth.Attributes;
using ToDoApi.Common.Core.Attributes;
using ToDoApi.Common.Core.CQRS.Commands;

namespace ToDoApi.Services.Identity.Core.Commands;

[Authorize]
public class SignOutCommand : ICommand
{
    [Required]
    [PayloadProperty("refreshToken")]
    public string RefreshToken { get; set; }
}