using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoApi.Common.Infrastructure.Exceptions;
using ToDoApi.Common.Infrastructure.Vault.Exceptions;
using ToDoApi.Common.Infrastructure.Vault.Internals;
using ToDoApi.Common.Infrastructure.Vault.Options;
using VaultSharp;
using VaultSharp.V1.AuthMethods.AppRole;

namespace ToDoApi.Common.Infrastructure.Vault;

public static class Extensions
{
    private const string VaultOptionsSectionName = "Vault";

    public static IHostBuilder UseVault(this IHostBuilder builder, string section = VaultOptionsSectionName)
    {
        return builder
            .ConfigureServices(services => services.AddVault(section))
            .ConfigureAppConfiguration((context, config) =>
            {
                var builtConfig = config.Build();
                var options = builtConfig.GetVaultOptions(section);

                config.AddVaultAsync(options).GetAwaiter().GetResult();
            });
    }

    private static IServiceCollection AddVault(this IServiceCollection services, string section)
    {
        if (string.IsNullOrWhiteSpace(section)) section = VaultOptionsSectionName;

        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();

        if (configuration is null)
            throw new ConfigurationException("Failed to configure Vault due to application configuration failure.");

        var options = configuration.GetVaultOptions(section);
        ValidateOptions(options);

        var (vaultSettings, vaultClient) = GetVaultClientWithSettings(options);

        services.AddSingleton(options);
        services.AddSingleton(vaultSettings);
        services.AddSingleton(vaultClient);
        services.AddSingleton<IKeyValueSecretProvider, KeyValueSecretProvider>();
        services.AddHostedService<VaultLeasingHostedService>();

        return services;
    }

    private static VaultOptions GetVaultOptions(this IConfiguration configuration, string sectionName)
    {
        var options = configuration.GetOptions<VaultOptions>();
        options.AppRole.SecretId = configuration.GetSection("TODOAPI_VAULT_SECRET_ID").Value;
        return options;
    }

    private static Task AddVaultAsync(this IConfigurationBuilder builder, VaultOptions options)
    {
        ValidateOptions(options);

        return Task.CompletedTask;
    }

    private static void ValidateOptions(VaultOptions options)
    {
        if (string.IsNullOrEmpty(options.Address))
            throw new VaultConfigurationException(
                "The host address is not specified. Expected value is: \"https://hostname:port\"");

        if (options.KeyValue == null) throw new VaultConfigurationException("Key-value engine is not configured.");

        if (string.IsNullOrEmpty(options.KeyValue.MountPoint))
            throw new VaultConfigurationException(
                "The mount path is not specified. Expected value is: \"ToDoApi/\"");

        if (options.KeyValue.EngineVersion > 2 || options.KeyValue.EngineVersion <= 0)
            throw new VaultConfigurationException(
                $"Invalid Secrets Engine version: {options.KeyValue.EngineVersion}. Available versions are 1 and 2.");

        if (options.AppRole is null) throw new VaultConfigurationException("AppRole configuration was not provided.");

        if (string.IsNullOrEmpty(options.AppRole.RoleId))
            throw new VaultConfigurationException("Role is not specified. Expected value is: \"role-name\"");
    }

    private static (VaultClientSettings settings, VaultClient client) GetVaultClientWithSettings(
        VaultOptions options
    )
    {
        var authMethodInfo = new AppRoleAuthMethodInfo(options.AppRole.RoleId, options.AppRole.SecretId);
        var settings = new VaultClientSettings(options.Address, authMethodInfo);
        var client = new VaultClient(settings);

        return (settings, client);
    }
}