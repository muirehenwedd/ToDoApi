using ToDoApi.Common.Infrastructure.Attributes;

namespace ToDoApi.Common.Infrastructure.Data.Postgres.Options;

[ConfigOptions("Postgres")]
public class PostgresOptions
{
    public bool ManualHost { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
}