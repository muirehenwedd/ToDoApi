using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ToDoApi.Common.Infrastructure.Vault.Options;
using VaultSharp;

namespace ToDoApi.Common.Infrastructure.Vault.Internals;

public class VaultLeasingHostedService : BackgroundService
{
    private readonly VaultClient _client;
    private readonly VaultOptions _options;

    public VaultLeasingHostedService(VaultOptions options, VaultClient client)
    {
        _options = options;
        _client = client;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_options.TokenTtl == 0)
            return;

        var ttlMillis = (_options.TokenTtl - 60) * 1000;

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(ttlMillis, stoppingToken);
            _client.V1.Auth.ResetVaultToken();
        }
    }
}