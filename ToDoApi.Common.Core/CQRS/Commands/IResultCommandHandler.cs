using System.Threading.Tasks;

namespace ToDoApi.Common.Core.CQRS.Commands;

public interface IResultCommandHandler<in TCommand, TResult> where TCommand : class, ICommand
{
    public Task<TResult> HandleAsync(TCommand command);
}