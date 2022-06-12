﻿using System;

namespace ToDoApi.Services.Tasks.Core.Queries.Responses;

public sealed class TaskQueryResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsDone { get; set; }
    public DateTime CreationTimestamp { get; set; }
}