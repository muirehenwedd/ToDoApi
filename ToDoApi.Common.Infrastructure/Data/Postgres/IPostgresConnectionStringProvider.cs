namespace ToDoApi.Common.Infrastructure.Data.Postgres;

public interface IPostgresConnectionStringProvider
{
    string ConnectionString { get; }
}