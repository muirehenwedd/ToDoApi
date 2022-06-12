using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ToDoApi.Common.Core.CQRS.Queries;
using ToDoApi.Common.Infrastructure.Exceptions;

namespace ToDoApi.Common.Infrastructure.CQRS.Queries.Internals;

internal class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceScopeFactory _scopeFactory;

    public QueryDispatcher(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<TResult> DispatchAsync<TResult>()
    {
        var scope = _scopeFactory.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var handler = serviceProvider.GetService<IEmptyQueryHandler<TResult>>();

        if (handler is null) throw new QueryHandlerNotRegisteredException(typeof(TResult));

        return await handler.HandleAsync();
    }

    public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : class, IQuery
    {
        var scope = _scopeFactory.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var handler = serviceProvider.GetService<IQueryHandler<TQuery, TResult>>();

        if (handler is null) throw new QueryHandlerNotRegisteredException(query);

        return await handler.HandleAsync(query);
    }
}