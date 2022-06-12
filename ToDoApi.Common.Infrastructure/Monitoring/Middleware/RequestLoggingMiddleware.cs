using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ToDoApi.Common.Core.Exceptions;
using ToDoApi.Common.Infrastructure.CQRS;
using ToDoApi.Common.Infrastructure.ErrorHandling;
using ToDoApi.Common.Infrastructure.Monitoring.LogEntry;

namespace ToDoApi.Common.Infrastructure.Monitoring.Middleware;

public sealed class RequestLoggingMiddleware
{
    private readonly IExceptionRegistry _exceptionRegistry;
    private readonly ILogger _logger;
    private readonly RequestDelegate _requestDelegate;

    public RequestLoggingMiddleware(
        RequestDelegate requestDelegate,
        ILogger logger,
        IExceptionRegistry exceptionRegistry
    )
    {
        _requestDelegate = requestDelegate;
        _logger = logger;
        _exceptionRegistry = exceptionRegistry;
    }

    public async Task Invoke(HttpContext context)
    {
        var start = Stopwatch.GetTimestamp();

        try
        {
            await _requestDelegate.Invoke(context);

            _logger.Information(new
            {
                method = context.Request.Method,
                uri = context.Request.Path.Value,
                statusCode = context.Response.StatusCode,
                elapsed = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()),
                clientIp = context.Connection.RemoteIpAddress?.ToString(),
                userAgent = context.Request.Headers["User-Agent"].FirstOrDefault() ?? "",
                identity = context.GetIdentityContext()
            });
        }
        catch (Exception exception)
        {
            if (!context.Response.HasStarted) return;

            var identityContext = context.GetIdentityContext();

            var stop = Stopwatch.GetTimestamp();
            var exceptionDispatchInfo = ExceptionDispatchInfo.Capture(exception);
            var (errorCode, _, _) = _exceptionRegistry.GetResponseData(exceptionDispatchInfo.SourceException);
            var exceptionScopeName =
                (exceptionDispatchInfo.SourceException as BaseException)?.ExceptionScope.Name ??
                ErrorScope.Global.Name;

            _logger.Error(new RequestErrorLogEntry
            {
                Method = context.Request.Method,
                Uri = context.Request.Path.Value,
                StatusCode = context.Response.StatusCode,
                Elapsed = GetElapsedMilliseconds(start, stop),
                ClientIp = context.Connection.RemoteIpAddress?.ToString(),
                UserAgent = context.Request.Headers["User-Agent"].FirstOrDefault(),
                Exception = exceptionDispatchInfo.SourceException.GetType().ToString(),
                ErrorCode = errorCode,
                ErrorScope = exceptionScopeName,
                ErrorMessage = exceptionDispatchInfo.SourceException.Message,
                StackTrace = exceptionDispatchInfo.SourceException.ToString(),
                Identity = identityContext
            }, exceptionDispatchInfo.SourceException);
        }
    }

    private static int GetElapsedMilliseconds(long start, long stop)
    {
        return (int) ((stop - start) * 1000L / (double) Stopwatch.Frequency);
    }
}