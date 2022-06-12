using System;
using System.Threading.Tasks;
using ToDoApi.Common.Core.Context;
using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Services.Tasks.Core.Commands.Responses;
using ToDoApi.Services.Tasks.Domain.Entities;
using ToDoApi.Services.Tasks.Domain.Repository;

namespace ToDoApi.Services.Tasks.Core.Commands.Handlers;

public class NewTaskCommandHandler : IResultCommandHandler<NewTaskCommand, NewTaskCommandResponse>
{
    private readonly ITodoTaskRepository _repository;
    private readonly IIdentityContext _identityContext;

    public NewTaskCommandHandler(ITodoTaskRepository repository, IIdentityContext identityContext)
    {
        _repository = repository;
        _identityContext = identityContext;
    }

    public async Task<NewTaskCommandResponse> HandleAsync(NewTaskCommand command)
    {
        var task = new ToDoTask()
        {
            Id = Guid.NewGuid(),
            User = _identityContext.UserId,
            Title = command.Title,
            Description = command.Description,
            CreationTimestamp = DateTime.UtcNow,
            IsDone = command.IsDone.HasValue && !command.IsDone.Value
        };
        
        await _repository.CommitAsync(_repository.Create(task));

        return new NewTaskCommandResponse
        {
            TaskId = task.Id
        };
    }
}