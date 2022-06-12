using ToDoApi.Common.Core.Exceptions;

namespace ToDoApi.Services.Tasks.Core.Exceptions;

public class TaskExceptionScope : IExceptionScope
{
    public TaskExceptionScope(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public static TaskExceptionScope TaskManagement => new("task-management");
}