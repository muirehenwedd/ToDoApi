using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoApi.Common.Infrastructure.Vault;

public interface IKeyValueSecretProvider
{
    public Task<TResult> GetValueAsync<TResult>(string path, string key, int? version = null);
    public Task<TResult> GetAsync<TResult>();
    public Task<TResult> GetAsync<TResult>(string path, int? version = null);
    public Task<IDictionary<string, object>> GetAsync(string path, int? version = null);
}