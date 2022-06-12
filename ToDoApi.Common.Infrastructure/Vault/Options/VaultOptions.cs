using ToDoApi.Common.Infrastructure.Attributes;

namespace ToDoApi.Common.Infrastructure.Vault.Options;

[ConfigOptions("Vault")]
public sealed class VaultOptions
{
    public string Address { get; set; }
    public int TokenTtl { get; set; }
    public KeyValueOptions KeyValue { get; set; }
    public AppRoleOptions AppRole { get; set; }
}