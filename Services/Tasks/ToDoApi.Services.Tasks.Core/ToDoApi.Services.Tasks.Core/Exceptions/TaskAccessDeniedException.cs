using System;
using ToDoApi.Common.Core.Exceptions;

namespace ToDoApi.Services.Tasks.Core.Exceptions;

public class TaskAccessDeniedException:BaseException
{
    public TaskAccessDeniedException(Guid taskId) : base(TaskExceptionScope.TaskManagement,
        $"You have no rights to interact with task '{taskId}'.")
    {
    }
}