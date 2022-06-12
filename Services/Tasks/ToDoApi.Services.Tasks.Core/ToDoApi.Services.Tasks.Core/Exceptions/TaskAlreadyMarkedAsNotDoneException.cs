using System;
using ToDoApi.Common.Core.Exceptions;

namespace ToDoApi.Services.Tasks.Core.Exceptions;

public class TaskAlreadyMarkedAsNotDoneException : BaseException
{
    public TaskAlreadyMarkedAsNotDoneException(Guid taskId) : base(TaskExceptionScope.TaskManagement,
        $"Task '{taskId}' already marked as not done.")
    {
    }
}