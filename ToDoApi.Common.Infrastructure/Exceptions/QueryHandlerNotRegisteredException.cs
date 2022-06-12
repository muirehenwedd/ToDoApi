using System;
using ToDoApi.Common.Core.CQRS.Queries;
using ToDoApi.Common.Core.Exceptions;
using ToDoApi.Common.Infrastructure.ErrorHandling;

namespace ToDoApi.Common.Infrastructure.Exceptions;

public class QueryHandlerNotRegisteredException : BaseException
{
    public QueryHandlerNotRegisteredException(Type resultType) : base(ErrorScope.QueryHandling,
        $"Query handler for query with result type '{resultType}' is not registered.")
    {
    }

    public QueryHandlerNotRegisteredException(IQuery query) : base(ErrorScope.QueryHandling,
        $"Query handler for query '{query.GetType()}' is not registered.")
    {
    }

    public QueryHandlerNotRegisteredException(IQuery query, Exception innerException) : base(
        ErrorScope.QueryHandling, $"Query handler for query '{query.GetType()}' is not registered.",
        innerException)
    {
    }
}