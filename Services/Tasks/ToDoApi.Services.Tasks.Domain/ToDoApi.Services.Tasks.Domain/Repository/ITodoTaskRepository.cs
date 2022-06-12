using System;
using ToDoApi.Common.Domain;
using ToDoApi.Services.Tasks.Domain.Entities;

namespace ToDoApi.Services.Tasks.Domain.Repository;

public interface ITodoTaskRepository : IRepository<ToDoTask, Guid>
{
}