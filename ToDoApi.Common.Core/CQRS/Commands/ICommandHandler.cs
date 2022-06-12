using System.Threading.Tasks;

namespace ToDoApi.Common.Core.CQRS.Commands;

public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
{
    public Task HandleAsync(TCommand command);
}