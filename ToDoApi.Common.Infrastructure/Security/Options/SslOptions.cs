using ToDoApi.Common.Infrastructure.Attributes;

namespace ToDoApi.Common.Infrastructure.Security.Options;

[ConfigOptions("SSL")]
public sealed class SslOptions
{
    public string Certificate { get; set; }
    public string PrivateKey { get; set; }
    public string RootAuthority { get; set; }
    public string PfxCertificate { get; set; }

    [EnvironmentVariable("SSL_PFX_PASSWORD")]
    public string PfxPassword { get; set; }
}