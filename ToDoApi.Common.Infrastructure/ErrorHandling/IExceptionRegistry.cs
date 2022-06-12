using System;
using System.Net;

namespace ToDoApi.Common.Infrastructure.ErrorHandling;

public interface IExceptionRegistry
{
    public void RegisterExceptionMapper(IExceptionMapper mapper);
    public (int, string, HttpStatusCode) GetResponseData(Exception exception);
}