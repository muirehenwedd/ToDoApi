using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ToDoApi.Common.Auth;
using ToDoApi.Common.Auth.Attributes;
using ToDoApi.Common.Auth.Contracts;
using ToDoApi.Common.Core.Context;
using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Common.Core.CQRS.Queries;
using ToDoApi.Common.Infrastructure.CQRS.Internals;
using ToDoApi.Common.Infrastructure.Exceptions.API;
using ToDoApi.Common.Infrastructure.Validation;

namespace ToDoApi.Common.Infrastructure.CQRS;

public static class HttpContextExtensions
{
    public static void Authorize<T>(this HttpContext context) where T : class
    {
        var dispatchItem = typeof(T);

        var authAttribute = dispatchItem.GetCustomAttribute<AuthorizeAttribute>();

        if (authAttribute is null)
            return;

        var tokenUsage = context.GetAuthorizationTokenUsage();

        if (tokenUsage != authAttribute.TokenUsage)
            throw new ForbiddenException();

        switch (tokenUsage)
        {
            case TokenUsage.Authorization:
                ValidateDefaultAuthorization(context, dispatchItem);
                break;

            case TokenUsage.Unset:
            default: return;
        }
    }

    private static void ValidateDefaultAuthorization(HttpContext context, Type dispatchItem)
    {
        var identity = context.GetIdentityContext();

        if (identity is null)
            throw new ForbiddenException();
    }

    public static async Task<T> ReadBodyAsync<T>(this HttpContext context)
    {
        if (context.Request.Body is null)
            throw new BadRequestException("Request body is invalid: missing request body.");

        var routeValues = context.GetRouteData().Values;

        using var reader = new StreamReader(context.Request.Body);
        var jsonBody = await reader.ReadToEndAsync();
        var bodyDictionary = JsonConvert.DeserializeObject<IDictionary<string, object>>(jsonBody);

        if (bodyDictionary is null)
            bodyDictionary = new Dictionary<string, object>();

        foreach (var routeValuePair in routeValues)
            bodyDictionary[routeValuePair.Key] = routeValuePair.Value;

        var validator = context.RequestServices.GetRequiredService<IPayloadValidator>();
        var validationResult = validator.Validate<T>(bodyDictionary);

        if (!validationResult.IsValid)
            throw new BadRequestException(typeof(T), validationResult);

        return validationResult.Payload;
    }

    public static Task<T> ReadQueryAsync<T>(this HttpContext context) where T : new()
    {
        var type = typeof(T);
        var values = context.GetRouteData().Values;
        var queryString = context.Request.QueryString.Value;

        if (queryString != null)
        {
            var query = HttpUtility.ParseQueryString(queryString);

            foreach (var key in query.AllKeys)
            {
                var value = query[key];

                if (key is null || value is null) continue;

                values.TryAdd(key, value);
            }
        }

        values.TryValidatePayload<T>();

        var serialized = JsonConvert.SerializeObject(values)
            .Replace("\\\"", "\"")
            .Replace("\"{", "{")
            .Replace("}\"", "}")
            .Replace("\"[", "[")
            .Replace("]\"", "]");

        var validator = context.RequestServices.GetRequiredService<IPayloadValidator>();
        var payload = JsonConvert.DeserializeObject<T>(serialized);

        if (payload is null)
            throw new BadRequestException($"Unable to parse request body of type '{typeof(T)}'.");

        var validationResult = validator.Validate<T>(values);

        if (!validationResult.IsValid)
            throw new BadRequestException(typeof(T), validationResult);

        return Task.FromResult(payload);
    }

    public static Task<TResult> QueryAsync<TResult>(this HttpContext context)
    {
        return context.RequestServices.GetRequiredService<IQueryDispatcher>().DispatchAsync<TResult>();
    }

    public static Task<TResult> QueryAsync<TQuery, TResult>(this HttpContext context, TQuery query)
        where TQuery : class, IQuery
    {
        return context.RequestServices.GetRequiredService<IQueryDispatcher>().DispatchAsync<TQuery, TResult>(query);
    }

    public static Task DispatchAsync<T>(this HttpContext context, T command) where T : class, ICommand
    {
        return context.RequestServices.GetRequiredService<ICommandDispatcher>().DispatchAsync(command);
    }

    public static Task<TResult> DispatchWithResultAsync<T, TResult>(this HttpContext context, T command)
        where T : class, ICommand
    {
        var dispatcher = context.RequestServices.GetRequiredService<ICommandDispatcher>();
        return dispatcher.DispatchWithResultAsync<T, TResult>(command);
    }

    public static Task WriteResponseAsync(this HttpContext context, HttpStatusCode statusCode, object body)
    {
        return WriteAsJsonAsync(context.Response, statusCode, body);
    }

    private static async Task WriteAsJsonAsync(this HttpResponse response, HttpStatusCode statusCode, object body)
    {
        response.ContentType = JsonConstants.JsonContentTypeWithCharset;
        response.StatusCode = (int) statusCode;

        var jsonString = JsonConvert.SerializeObject(body, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });

        var writer = new StreamWriter(response.Body, Encoding.UTF8);

        try
        {
            await writer.WriteAsync(jsonString);
            await writer.FlushAsync();
        }
        finally
        {
            await writer.DisposeAsync();
        }
    }

    public static void SetStatusCode(this HttpContext context, HttpStatusCode statusCode)
    {
        context.Response.StatusCode = (int) statusCode;
    }

    public static IIdentityContext? GetIdentityContext(this HttpContext context)
    {
        try
        {
            return context.RequestServices.GetRequiredService<IIdentityContext>();
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static TokenUsage GetAuthorizationTokenUsage(this HttpContext context)
    {
        if (context.Items[IdentityItems.TokenUsage] is TokenUsage tokenUsage)
            return tokenUsage;

        return TokenUsage.Unset;
    }
}