using System;
using ToDoApi.Common.Domain;

namespace ToDoApi.Services.Tasks.Domain.Entities;

public class ToDoTask:AggregateRoot
{
    public Guid User { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsDone { get; set; }
    public DateTime CreationTimestamp { get; set; }
}