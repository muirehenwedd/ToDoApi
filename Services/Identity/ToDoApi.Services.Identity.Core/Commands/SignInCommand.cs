using ToDoApi.Common.Core.Attributes;
using ToDoApi.Common.Core.CQRS.Commands;

namespace ToDoApi.Services.Identity.Core.Commands;

public class SignInCommand : ICommand
{
    [Required]
    [PayloadProperty("login")]
    public string Login { get; set; }

    [Required]
    [PayloadProperty("password")]
    public string Password { get; set; }
}