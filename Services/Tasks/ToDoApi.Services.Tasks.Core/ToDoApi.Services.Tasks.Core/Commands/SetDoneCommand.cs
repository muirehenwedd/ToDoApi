using System;
using ToDoApi.Common.Auth.Attributes;
using ToDoApi.Common.Core.Attributes;
using ToDoApi.Common.Core.CQRS.Commands;

namespace ToDoApi.Services.Tasks.Core.Commands;

[Authorize]
public class SetDoneCommand : ICommand
{
    [Required]
    [PayloadProperty("taskId")]
    public Guid TaskId { get; set; }
}