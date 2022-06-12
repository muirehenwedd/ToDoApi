using System;
using System.Net;

namespace ToDoApi.Common.Infrastructure.ErrorHandling;

public interface IExceptionMapper
{
    (int, string, HttpStatusCode) Map(Exception exception);
}