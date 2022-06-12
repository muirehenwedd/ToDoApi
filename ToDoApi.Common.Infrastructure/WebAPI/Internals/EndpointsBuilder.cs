using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Common.Core.CQRS.Queries;
using ToDoApi.Common.Infrastructure.CQRS;
using ToDoApi.Common.Infrastructure.CQRS.Commands;

namespace ToDoApi.Common.Infrastructure.WebAPI.Internals;

internal class EndpointsBuilder : IEndpointsBuilder
{
    private readonly IEndpointRouteBuilder _routeBuilder;

    public EndpointsBuilder(IEndpointRouteBuilder routeBuilder)
    {
        _routeBuilder = routeBuilder;
    }

    public IEndpointsBuilder Get<TQuery, TResult>(string route, Func<TQuery, HttpContext, Task<TResult>> handler)
        where TQuery : class, IQuery, new()
    {
        _routeBuilder
            .MapGet(route, context => BuildQueryContext(context, handler));

        return this;
    }

    public IEndpointsBuilder Post<TBody>(string route, Func<TBody, HttpContext, Task> handler)
        where TBody : class, ICommand
    {
        _routeBuilder
            .MapPost(route, context => BuildCommandContext(context, handler));

        return this;
    }

    public IEndpointsBuilder Post<TBody, TResult>(string route, Func<TBody, HttpContext, Task<TResult>> handler)
        where TBody : class, ICommand
    {
        _routeBuilder
            .MapPost(route, context => BuildCommandContext(context, handler));

        return this;
    }

    public IEndpointsBuilder Put<TBody>(string route, Func<TBody, HttpContext, Task> handler)
        where TBody : class, ICommand
    {
        _routeBuilder
            .MapPut(route, context => BuildCommandContext(context, handler));

        return this;
    }

    public IEndpointsBuilder Put<TBody, TResult>(string route, Func<TBody, HttpContext, Task<TResult>> handler)
        where TBody : class, ICommand
    {
        _routeBuilder
            .MapPut(route, context => BuildCommandContext(context, handler));

        return this;
    }

    public IEndpointsBuilder Delete<TBody>(string route, Func<TBody, HttpContext, Task> handler)
        where TBody : class, ICommand
    {
        _routeBuilder
            .MapDelete(route, context => BuildCommandContext(context, handler));

        return this;
    }

    public IEndpointsBuilder Delete<TBody, TResult>(string route, Func<TBody, HttpContext, Task<TResult>> handler)
        where TBody : class, ICommand
    {
        _routeBuilder
            .MapDelete(route, context => BuildCommandContext(context, handler));

        return this;
    }

    private static async Task BuildQueryContext<TBody, TResult>(
        HttpContext context,
        Func<TBody, HttpContext, Task<TResult>> handler
    )
        where TBody : class, IQuery, new()
    {
        context.Authorize<TBody>();

        var query = await context.ReadQueryAsync<TBody>();
        var result = await handler.Invoke(query, context);
        await context.WriteResponseAsync(HttpStatusCode.OK, result);
    }

    private static async Task BuildCommandContext<TBody>(
        HttpContext context,
        Func<TBody, HttpContext, Task> handler
    ) where TBody : class, ICommand
    {
        context.Authorize<TBody>();

        var command = context.ReadBodyAsync<TBody>().GetAwaiter().GetResult();
        await handler.Invoke(command, context);
        var statusRegistry = context.RequestServices.GetRequiredService<ICommandStatusRegistry>();
        context.SetStatusCode(statusRegistry.GetStatusCode(command));
    }

    private static Task BuildCommandContext<TBody, TResult>(
        HttpContext context,
        Func<TBody, HttpContext, Task<TResult>> handler
    ) where TBody : class, ICommand
    {
        context.Authorize<TBody>();

        var command = context.ReadBodyAsync<TBody>().GetAwaiter().GetResult();
        var result = handler.Invoke(command, context).GetAwaiter().GetResult();
        var statusRegistry = context.RequestServices.GetRequiredService<ICommandStatusRegistry>();
        return context.WriteResponseAsync(statusRegistry.GetStatusCode(command), result);
    }
}