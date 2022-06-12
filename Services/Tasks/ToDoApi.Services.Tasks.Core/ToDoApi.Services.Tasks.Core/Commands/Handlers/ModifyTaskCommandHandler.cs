using System.Threading.Tasks;
using ToDoApi.Common.Core.Context;
using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Services.Tasks.Core.Commands.Responses;
using ToDoApi.Services.Tasks.Core.Exceptions;
using ToDoApi.Services.Tasks.Domain.Repository;

namespace ToDoApi.Services.Tasks.Core.Commands.Handlers;

public class ModifyTaskCommandHandler : IResultCommandHandler<ModifyTaskCommand, ModifyTaskCommandResponse>
{
    private readonly ITodoTaskRepository _repository;
    private readonly IIdentityContext _identityContext;

    public ModifyTaskCommandHandler(ITodoTaskRepository repository, IIdentityContext identityContext)
    {
        _repository = repository;
        _identityContext = identityContext;
    }

    public async Task<ModifyTaskCommandResponse> HandleAsync(ModifyTaskCommand command)
    {
        var task = await _repository.GetById(command.TaskId);
        if (task is null)
            throw new TaskNotFoundException(command.TaskId);

        if (task.User != _identityContext.UserId)
            throw new TaskAccessDeniedException(command.TaskId);

        await _repository.CommitAsync(Task.Run(() =>
        {
            task.Title = command.Title ?? task.Title;
            task.Description = command.Description ?? task.Description;
        }));

        return new ModifyTaskCommandResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            CreationTimestamp = task.CreationTimestamp,
            IsDone = task.IsDone
        };
    }
}