using ToDoApi.Common.Core.Exceptions;
using ToDoApi.Common.Infrastructure.ErrorHandling;

namespace ToDoApi.Common.Infrastructure.Data.Postgres.Exceptions;

public class PostgresConfigurationException : BaseException
{
    public PostgresConfigurationException(string message) : base(ErrorScope.Postgres, message, null)
    {
    }
}