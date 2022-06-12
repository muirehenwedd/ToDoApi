using System;
using ToDoApi.Common.Core.Exceptions;

namespace ToDoApi.Common.Infrastructure.ErrorHandling;

public class ErrorScope : IExceptionScope
{
    internal ErrorScope(string name)
    {
        Name = name;
    }

    public static ErrorScope Identity => new("identity");
    public static ErrorScope Spaces => new("spaces");
    public static ErrorScope Global => new("global");
    public static ErrorScope Vault => new("vault");
    public static ErrorScope Postgres => new("postgres");
    public static ErrorScope Mongo => new("mongo");
    public static ErrorScope Redis => new("redis");
    public static ErrorScope CommandHandling => new("command-handling");
    public static ErrorScope QueryHandling => new("query-handling");
    public static ErrorScope RequestHandling => new("request-handling");
    public static ErrorScope Postal => new("postal");
    public static ErrorScope ContentServer => new("content-server");

    public string Name { get; }

    public BaseException CreateException(string message, Exception innerException = null)
    {
        return new BaseException(this, message, innerException);
    }
}