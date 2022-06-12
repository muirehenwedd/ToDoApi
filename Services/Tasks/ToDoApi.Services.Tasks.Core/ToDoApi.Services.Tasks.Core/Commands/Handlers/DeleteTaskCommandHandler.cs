using System.Threading.Tasks;
using ToDoApi.Common.Core.Context;
using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Services.Tasks.Core.Exceptions;
using ToDoApi.Services.Tasks.Domain.Repository;

namespace ToDoApi.Services.Tasks.Core.Commands.Handlers;

public class DeleteTaskCommandHandler : ICommandHandler<DeleteTaskCommand>
{
    private readonly ITodoTaskRepository _repository;
    private readonly IIdentityContext _identityContext;

    public DeleteTaskCommandHandler(ITodoTaskRepository repository, IIdentityContext identityContext)
    {
        _repository = repository;
        _identityContext = identityContext;
    }

    public async Task HandleAsync(DeleteTaskCommand command)
    {
        var task = await _repository.GetById(command.TaskId);
        if (task is null)
            throw new TaskNotFoundException(command.TaskId);

        if (task.User != _identityContext.UserId)
            throw new TaskAccessDeniedException(command.TaskId);

        await _repository.CommitAsync(_repository.Delete(task));
    }
}