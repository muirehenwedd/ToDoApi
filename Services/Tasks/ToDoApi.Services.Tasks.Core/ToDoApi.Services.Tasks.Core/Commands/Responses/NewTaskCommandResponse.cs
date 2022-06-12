using System;

namespace ToDoApi.Services.Tasks.Core.Commands.Responses;

public class NewTaskCommandResponse
{
    public Guid TaskId { get; set; }
}