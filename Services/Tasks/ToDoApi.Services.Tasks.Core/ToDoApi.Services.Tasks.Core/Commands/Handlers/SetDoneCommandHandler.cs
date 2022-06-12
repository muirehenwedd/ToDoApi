using System.Threading.Tasks;
using ToDoApi.Common.Core.Context;
using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Services.Tasks.Core.Exceptions;
using ToDoApi.Services.Tasks.Domain.Repository;

namespace ToDoApi.Services.Tasks.Core.Commands.Handlers;

public class SetDoneCommandHandler : ICommandHandler<SetDoneCommand>
{
    private readonly ITodoTaskRepository _repository;
    private readonly IIdentityContext _identityContext;

    public SetDoneCommandHandler(ITodoTaskRepository repository, IIdentityContext identityContext)
    {
        _repository = repository;
        _identityContext = identityContext;
    }

    public async Task HandleAsync(SetDoneCommand command)
    {
        var task = await _repository.GetById(command.TaskId);
        if (task is null)
            throw new TaskNotFoundException(command.TaskId);

        if (task.User != _identityContext.UserId)
            throw new TaskAccessDeniedException(command.TaskId);

        if (task.IsDone)
            throw new TaskAlreadyMarkedAsDoneException(task.Id);

        await _repository.CommitAsync(Task.Run(() => { task.IsDone = true; }));
    }
}