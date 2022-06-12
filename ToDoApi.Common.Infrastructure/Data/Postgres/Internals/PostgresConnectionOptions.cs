using System;
using ToDoApi.Common.Infrastructure.Vault;
using ToDoApi.Common.Infrastructure.Vault.Attributes;

namespace ToDoApi.Common.Infrastructure.Data.Postgres.Internals;

[Serializable]
[VaultSecret("postgres")]
public sealed class PostgresConnectionOptions
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Database { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool Pooling { get; set; }
    public int MinPoolSize { get; set; }
    public int MaxPoolSize { get; set; }
    public string SslMode { get; set; }
    public bool TrustServerCertificate { get; set; }
}