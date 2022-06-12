using System;
using System.Net;
using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Common.Infrastructure.Attributes;
using ToDoApi.Common.Infrastructure.CQRS.Commands;

namespace ToDoApi.Services.Tasks.Core.Commands;

[CommandStatusMapper]
public class TaskCommandStatusMapper : ICommandStatusMapper
{
    public HttpStatusCode Map(ICommand command)
    {
        return command switch
        {
            NewTaskCommand => HttpStatusCode.Created,
            _ => throw new ArgumentOutOfRangeException(nameof(command), command, null)
        };
    }
}