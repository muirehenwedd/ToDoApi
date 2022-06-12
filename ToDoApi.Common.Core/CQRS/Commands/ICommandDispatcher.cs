using System.Threading.Tasks;

namespace ToDoApi.Common.Core.CQRS.Commands;

public interface ICommandDispatcher
{
    public Task DispatchAsync<TCommand>(TCommand command) where TCommand : class, ICommand;
    public Task<TResult> DispatchWithResultAsync<TCommand, TResult>(TCommand command) where TCommand : class, ICommand;
}