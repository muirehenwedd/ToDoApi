using System;
using ToDoApi.Common.Auth.Attributes;
using ToDoApi.Common.Core.Attributes;
using ToDoApi.Common.Core.CQRS.Queries;

namespace ToDoApi.Services.Tasks.Core.Queries;

[Authorize]
public class TaskQuery : IQuery
{
    [Required]
    [PayloadProperty("taskId")]
    public Guid TaskId { get; set; }
}