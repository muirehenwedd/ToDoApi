using System;
using System.Collections.Generic;

namespace ToDoApi.Services.Tasks.Core.Queries.Responses;

public sealed class AllTasksQueryResponse
{
    public sealed class TaskInfo
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsDone { get; set; }
        public DateTime CreationTimestamp { get; set; }
    }

    public IEnumerable<TaskInfo> Tasks { get; set; }
}