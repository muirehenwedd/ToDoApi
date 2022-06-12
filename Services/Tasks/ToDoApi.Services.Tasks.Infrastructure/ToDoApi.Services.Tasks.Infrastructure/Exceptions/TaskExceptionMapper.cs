using System;
using System.Net;
using ToDoApi.Common.Infrastructure.Attributes;
using ToDoApi.Common.Infrastructure.ErrorHandling;
using ToDoApi.Services.Tasks.Core.Exceptions;

namespace ToDoApi.Services.Tasks.Infrastructure.Exceptions;

[ExceptionMapper]
public class TaskExceptionMapper : IExceptionMapper
{
    public (int, string, HttpStatusCode) Map(Exception exception)
    {
        return exception switch
        {
            TaskNotFoundException => (301, exception.Message, HttpStatusCode.NotFound),
            TaskAccessDeniedException => (302, exception.Message, HttpStatusCode.Forbidden),
            TaskAlreadyMarkedAsDoneException => (303, exception.Message, HttpStatusCode.Conflict),
            TaskAlreadyMarkedAsNotDoneException => (304, exception.Message, HttpStatusCode.Conflict),

            _ => throw new ArgumentOutOfRangeException(nameof(exception), exception, null)
        };
    }
}