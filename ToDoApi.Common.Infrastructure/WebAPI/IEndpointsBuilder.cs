using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Common.Core.CQRS.Queries;

namespace ToDoApi.Common.Infrastructure.WebAPI;

public interface IEndpointsBuilder
{
    IEndpointsBuilder Get<TQuery, TResult>(string route, Func<TQuery, HttpContext, Task<TResult>> handler)
        where TQuery : class, IQuery, new();

    IEndpointsBuilder Post<TBody>(string route, Func<TBody, HttpContext, Task> handler) where TBody : class, ICommand;

    IEndpointsBuilder Post<TBody, TResult>(string route, Func<TBody, HttpContext, Task<TResult>> handler)
        where TBody : class, ICommand;

    IEndpointsBuilder Put<TBody>(string route, Func<TBody, HttpContext, Task> handler) where TBody : class, ICommand;

    IEndpointsBuilder Put<TBody, TResult>(string route, Func<TBody, HttpContext, Task<TResult>> handler)
        where TBody : class, ICommand;

    IEndpointsBuilder Delete<TBody>(string route, Func<TBody, HttpContext, Task> handler) where TBody : class, ICommand;

    IEndpointsBuilder Delete<TBody, TResult>(string route, Func<TBody, HttpContext, Task<TResult>> handler)
        where TBody : class, ICommand;
}