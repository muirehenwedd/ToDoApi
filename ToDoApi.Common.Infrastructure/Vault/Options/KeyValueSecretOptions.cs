namespace ToDoApi.Common.Infrastructure.Vault.Options;

public sealed class KeyValueSecretOptions
{
    public string Path { get; set; }
    public int? Version { get; set; }
}