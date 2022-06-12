using ToDoApi.Common.Core.Attributes;
using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Common.Infrastructure.Validation;
using ToDoApi.Common.Infrastructure.Validation.Attributes;

namespace ToDoApi.Services.Identity.Core.Commands;

public class RegistrationCommand : ICommand
{
    [Required]
    [PayloadProperty("login")]
    public string Login { get; set; }

    [Required]
    [PayloadProperty("password")]
    [StringLength(NumericComparison.GreaterOrEqualTo, 8)]
    public string Password { get; set; }
}