using System;
using ToDoApi.Common.Core.Exceptions;

namespace ToDoApi.Services.Tasks.Core.Exceptions;

public class TaskAlreadyMarkedAsDoneException : BaseException
{
    public TaskAlreadyMarkedAsDoneException(Guid taskId) : base(TaskExceptionScope.TaskManagement,
        $"Task '{taskId}' already marked as done.")
    {
    }
}