using System;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ToDoApi.Common.Infrastructure.CQRS;
using ToDoApi.Common.Infrastructure.ErrorHandling.Internals;
using ToDoApi.Common.Infrastructure.Exceptions.API;

namespace ToDoApi.Common.Infrastructure.ErrorHandling.Middleware;

public sealed class ErrorHandlingMiddleware
{
    private readonly IExceptionRegistry _exceptionRegistry;
    private readonly RequestDelegate _requestDelegate;

    public ErrorHandlingMiddleware(RequestDelegate requestDelegate, IExceptionRegistry exceptionRegistry)
    {
        _requestDelegate = requestDelegate;
        _exceptionRegistry = exceptionRegistry;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _requestDelegate.Invoke(context);
        }
        catch (Exception exception)
        {
            var (errorCode, message, statusCode) = GetExceptionResponseData(context, exception);
            var responseBody = new ErrorResponse(errorCode, message);

            if (exception is BadRequestException badRequest)
                responseBody.ValidationResult = badRequest.ValidationResult;

            await context.WriteResponseAsync(statusCode, responseBody);

            throw;
        }
    }

    private (int errorCode, string message, HttpStatusCode statusCode) GetExceptionResponseData(
        HttpContext context,
        Exception exception
    )
    {
        var exceptionDispatchInfo = ExceptionDispatchInfo.Capture(exception);

        if (context.Response.HasStarted) exceptionDispatchInfo.Throw();

        return _exceptionRegistry.GetResponseData(exceptionDispatchInfo.SourceException);
    }
}