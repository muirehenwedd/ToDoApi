using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ToDoApi.Common.Infrastructure.Vault.Attributes;
using ToDoApi.Common.Infrastructure.Vault.Exceptions;
using ToDoApi.Common.Infrastructure.Vault.Options;
using VaultSharp;

namespace ToDoApi.Common.Infrastructure.Vault.Internals;

internal sealed class KeyValueSecretProvider : IKeyValueSecretProvider
{
    private readonly VaultClient _client;
    private readonly VaultOptions _options;

    public KeyValueSecretProvider(VaultOptions options, VaultClient client)
    {
        _options = options;
        _client = client;
    }

    public async Task<TResult> GetValueAsync<TResult>(string path, string key, int? version = null)
    {
        return JsonConvert.DeserializeObject<TResult>(JsonConvert.SerializeObject((await GetAsync(path, version))[key]))
            !;
    }

    public Task<TResult> GetAsync<TResult>()
    {
        var type = typeof(TResult);
        var attribute = type.GetCustomAttribute<VaultSecretAttribute>();

        if (attribute is null)
            throw new VaultRuntimeException($"Unable to query '{type}': VaultSecretAttribute is not set.");

        return GetAsync<TResult>(attribute.Path, attribute.Version);
    }

    public async Task<TResult> GetAsync<TResult>(string path, int? version = null)
    {
        return JsonConvert.DeserializeObject<TResult>(JsonConvert.SerializeObject(await GetAsync(path, version)))!;
    }

    public async Task<IDictionary<string, object>> GetAsync(string path, int? version = null)
    {
        if (string.IsNullOrEmpty(path)) throw new VaultRuntimeException("Key-value path is not specified.");

        try
        {
            switch (_options.KeyValue.EngineVersion)
            {
                case 1:
                    var secretV1 = await _client.V1.Secrets.KeyValue.V1.ReadSecretAsync(path,
                        _options.KeyValue.MountPoint);
                    return secretV1.Data;

                case 2:
                    var secretV2 = await _client.V1.Secrets.KeyValue.V2.ReadSecretAsync(path, version,
                        _options.KeyValue.MountPoint);
                    return secretV2.Data.Data;

                default:
                    throw new VaultRuntimeException(
                        $"Invalid key-value engine version: {_options.KeyValue.EngineVersion}.");
            }
        }
        catch (Exception exception)
        {
            throw new VaultRuntimeException($"Getting value for key path \"{path}\" caused an error.", exception);
        }
    }
}