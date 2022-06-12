using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Common.Core.CQRS.Queries;

namespace ToDoApi.Common.Infrastructure.CQRS;

public interface IServiceEndpointsBuilder
{
    public IServiceEndpointsBuilder Get<TQuery, TResponse>(string route) where TQuery : class, IQuery, new();

    public IServiceEndpointsBuilder Post<TCommand>(string route)
        where TCommand : class, ICommand;

    public IServiceEndpointsBuilder Post<TCommand, TResponse>(string route)
        where TCommand : class, ICommand;

    public IServiceEndpointsBuilder Put<TCommand>(string route)
        where TCommand : class, ICommand;

    public IServiceEndpointsBuilder Put<TCommand, TResponse>(string route)
        where TCommand : class, ICommand;

    public IServiceEndpointsBuilder Delete<TCommand>(string route)
        where TCommand : class, ICommand;

    public IServiceEndpointsBuilder Delete<TCommand, TResponse>(string route)
        where TCommand : class, ICommand;
}