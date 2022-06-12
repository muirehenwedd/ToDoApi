using System;

namespace ToDoApi.Common.Infrastructure.Vault.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class VaultSecretAttribute : Attribute
{
    public VaultSecretAttribute(string path)
    {
        Path = path;
    }

    public VaultSecretAttribute(string path, int version)
    {
        Path = path;
        Version = version;
    }

    public string Path { get; set; }
    public int? Version { get; set; }
}