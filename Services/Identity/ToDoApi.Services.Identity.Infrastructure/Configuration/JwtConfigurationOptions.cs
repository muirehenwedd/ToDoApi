using System;

using ToDoApi.Common.Infrastructure.Vault.Attributes;

namespace ToDoApi.Services.Identity.Infrastructure.Configuration;

[Serializable]
[VaultSecret("jwt")]
public sealed class JwtConfigurationOptions
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int TtlMinutes { get; set; }
    public int PasswordResetTtlMinutes { get; set; }
}