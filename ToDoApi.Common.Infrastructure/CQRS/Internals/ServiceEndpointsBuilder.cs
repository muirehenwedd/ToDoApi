using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Common.Core.CQRS.Queries;
using ToDoApi.Common.Infrastructure.WebAPI;

namespace ToDoApi.Common.Infrastructure.CQRS.Internals;

internal class ServiceEndpointsBuilder : IServiceEndpointsBuilder
{
    private readonly IEndpointsBuilder _endpointsBuilder;

    public ServiceEndpointsBuilder(IEndpointsBuilder endpointsBuilder)
    {
        _endpointsBuilder = endpointsBuilder;
    }

    public IServiceEndpointsBuilder Get<TQuery, TResponse>(string route) where TQuery : class, IQuery, new()
    {
        _endpointsBuilder.Get<TQuery, TResponse>(route,
            (query, context) => context.QueryAsync<TQuery, TResponse>(query));
        return this;
    }

    public IServiceEndpointsBuilder Post<TCommand>(string route)
        where TCommand : class, ICommand
    {
        _endpointsBuilder.Post<TCommand>(route, ExecuteCommand);
        return this;
    }

    public IServiceEndpointsBuilder Post<TCommand, TResponse>(string route)
        where TCommand : class, ICommand
    {
        _endpointsBuilder.Post<TCommand, TResponse>(route, ExecuteCommandWithResult<TCommand, TResponse>);
        return this;
    }

    public IServiceEndpointsBuilder Put<TCommand>(string route) where TCommand : class, ICommand
    {
        _endpointsBuilder.Put<TCommand>(route, ExecuteCommand);
        return this;
    }

    public IServiceEndpointsBuilder Put<TCommand, TResponse>(string route)
        where TCommand : class, ICommand
    {
        _endpointsBuilder.Put<TCommand, TResponse>(route, ExecuteCommandWithResult<TCommand, TResponse>);
        return this;
    }

    public IServiceEndpointsBuilder Delete<TCommand>(string route) where TCommand : class, ICommand
    {
        _endpointsBuilder.Delete<TCommand>(route, ExecuteCommand);
        return this;
    }

    public IServiceEndpointsBuilder Delete<TCommand, TResponse>(string route)
        where TCommand : class, ICommand
    {
        _endpointsBuilder.Delete<TCommand, TResponse>(route, ExecuteCommandWithResult<TCommand, TResponse>);
        return this;
    }

    private static Task ExecuteCommand<TCommand>(TCommand command, HttpContext context)
        where TCommand : class, ICommand
    {
        return context.DispatchAsync(command);
    }

    private static Task<TResult> ExecuteCommandWithResult<TCommand, TResult>(TCommand command, HttpContext context)
        where TCommand : class, ICommand
    {
        return context.DispatchWithResultAsync<TCommand, TResult>(command);
    }
}