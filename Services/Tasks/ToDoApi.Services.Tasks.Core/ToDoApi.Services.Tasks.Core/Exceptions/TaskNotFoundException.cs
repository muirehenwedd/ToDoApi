using System;
using ToDoApi.Common.Core.Exceptions;

namespace ToDoApi.Services.Tasks.Core.Exceptions;

public class TaskNotFoundException:BaseException
{
    public TaskNotFoundException(Guid taskId) : base(TaskExceptionScope.TaskManagement,
        $"Task with id '{taskId}' was not found.")
    {
    }
}