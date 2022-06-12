using ToDoApi.Common.Auth.Attributes;
using ToDoApi.Common.Core.Attributes;
using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Common.Infrastructure.Validation.Attributes;

namespace ToDoApi.Services.Tasks.Core.Commands;

[Authorize]
public class NewTaskCommand : ICommand
{
    [Required]
    [PayloadProperty("title")]
    [NotEmptyString]
    public string Title { get; set; }

    [PayloadProperty("description")]
    [NotEmptyString]
    public string? Description { get; set; }

    [PayloadProperty("isDone")]
    public bool? IsDone { get; set; }
}