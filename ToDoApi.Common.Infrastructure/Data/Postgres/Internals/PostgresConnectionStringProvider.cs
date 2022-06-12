using System.Text;
using System.Threading.Tasks;
using ToDoApi.Common.Infrastructure.Data.Postgres.Options;
using ToDoApi.Common.Infrastructure.Security.Options;
using ToDoApi.Common.Infrastructure.Vault;

namespace ToDoApi.Common.Infrastructure.Data.Postgres.Internals;

internal class PostgresConnectionStringProvider : IPostgresConnectionStringProvider
{
    private readonly IKeyValueSecretProvider _keyValueSecretProvider;
    private readonly PostgresOptions _postgresOptions;
    private readonly SslOptions _sslOptions;
    private string _connectionString;

    public PostgresConnectionStringProvider(
        SslOptions sslOptions,
        IKeyValueSecretProvider keyValueSecretProvider,
        PostgresOptions postgresOptions
    )
    {
        _sslOptions = sslOptions;
        _keyValueSecretProvider = keyValueSecretProvider;
        _postgresOptions = postgresOptions;
    }

    public string ConnectionString => _connectionString ??= BuildConnectionString().GetAwaiter().GetResult();

    private async Task<string> BuildConnectionString()
    {
        var options = await _keyValueSecretProvider.GetAsync<PostgresConnectionOptions>();
        var host = _postgresOptions.ManualHost ? _postgresOptions.Host : options.Host;
        var port = _postgresOptions.ManualHost ? _postgresOptions.Port : options.Port;

        var builder = new StringBuilder();
        builder.Append($"Host={host};");
        builder.Append($"Port={port};");
        builder.Append($"Database={options.Database};");
        builder.Append($"Username={options.Username};");
        builder.Append($"Password={options.Password};");
        builder.Append($"SSL Mode={options.SslMode};");
        builder.Append($"Trust Server Certificate={options.TrustServerCertificate};");
        builder.Append($"Root Certificate={_sslOptions.RootAuthority};");
        builder.Append($"Client Certificate={_sslOptions.Certificate};");
        builder.Append($"Client Certificate Key={_sslOptions.PrivateKey};");
        builder.Append($"Pooling={options.Pooling};");
        builder.Append($"Minimum Pool Size={options.MinPoolSize};");
        builder.Append($"Maximum Pool Size={options.MaxPoolSize};");
        builder.Append("include error detail=true");
        return builder.ToString();
    }
}