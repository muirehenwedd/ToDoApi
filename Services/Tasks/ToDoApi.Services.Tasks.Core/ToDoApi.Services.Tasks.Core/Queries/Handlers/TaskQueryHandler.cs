using System.Threading.Tasks;
using ToDoApi.Common.Core.Context;
using ToDoApi.Common.Core.CQRS.Queries;
using ToDoApi.Services.Tasks.Core.Exceptions;
using ToDoApi.Services.Tasks.Core.Queries.Responses;
using ToDoApi.Services.Tasks.Domain.Repository;

namespace ToDoApi.Services.Tasks.Core.Queries.Handlers;

public class TaskQueryHandler : IQueryHandler<TaskQuery, TaskQueryResponse>
{
    private readonly ITodoTaskRepository _repository;
    private readonly IIdentityContext _identityContext;

    public TaskQueryHandler(ITodoTaskRepository repository, IIdentityContext identityContext)
    {
        _repository = repository;
        _identityContext = identityContext;
    }

    public async Task<TaskQueryResponse> HandleAsync(TaskQuery query)
    {
        var task = await _repository.GetById(query.TaskId);
        if (task is null)
            throw new TaskNotFoundException(query.TaskId);

        if (task.User != _identityContext.UserId)
            throw new TaskAccessDeniedException(query.TaskId);

        return new TaskQueryResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            CreationTimestamp = task.CreationTimestamp,
            IsDone = task.IsDone
        };
    }
}