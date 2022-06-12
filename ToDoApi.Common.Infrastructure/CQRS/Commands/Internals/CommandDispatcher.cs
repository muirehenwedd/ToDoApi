using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Common.Infrastructure.Exceptions;

namespace ToDoApi.Common.Infrastructure.CQRS.Commands.Internals;

internal class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CommandDispatcher(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task DispatchAsync<TCommand>(TCommand command) where TCommand : class, ICommand
    {
        var serviceScope = _serviceScopeFactory.CreateScope();
        var handler = serviceScope.ServiceProvider.GetService<ICommandHandler<TCommand>>();

        if (handler is null) throw new CommandHandlerNotRegisteredException(command);

        return handler.HandleAsync(command);
    }

    public Task<TResult> DispatchWithResultAsync<TCommand, TResult>(TCommand command) where TCommand : class, ICommand
    {
        var serviceScope = _serviceScopeFactory.CreateScope();
        var handler = serviceScope.ServiceProvider.GetService<IResultCommandHandler<TCommand, TResult>>();

        if (handler is null) throw new CommandHandlerNotRegisteredException(command);

        return handler.HandleAsync(command);
    }
}