using System;

namespace ToDoApi.Services.Tasks.Core.Commands.Responses;

public class ModifyTaskCommandResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsDone { get; set; }
    public DateTime CreationTimestamp { get; set; }
}