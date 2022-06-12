using System;
using System.Collections.Generic;
using System.Net;

namespace ToDoApi.Common.Infrastructure.ErrorHandling.Internals;

internal class ExceptionRegistry : IExceptionRegistry
{
    private readonly IList<MapExceptionDelegate> _registry;

    public ExceptionRegistry()
    {
        _registry = new List<MapExceptionDelegate>();
    }

    public void RegisterExceptionMapper(IExceptionMapper mapper)
    {
        _registry.Add(mapper.Map);
    }

    public (int, string, HttpStatusCode) GetResponseData(Exception exception)
    {
        return TryGetResponseData(exception);
    }

    private (int, string, HttpStatusCode) TryGetResponseData(Exception exception)
    {
        foreach (var mapExceptionDelegate in _registry)
            try
            {
                return mapExceptionDelegate.Invoke(exception);
            }
            catch
            {
                // TODO: add exception logging
                // ignored
            }

        return ((int) GlobalErrorCodes.Unknown, "Unknown error encountered.", HttpStatusCode.InternalServerError);
    }

    private delegate (int, string, HttpStatusCode) MapExceptionDelegate(Exception exception);
}