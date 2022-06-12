using System;
using System.Security.Cryptography;
using System.Text;
using ToDoApi.Common.Infrastructure.Vault;
using ToDoApi.Common.Utility.Extensions;
using ToDoApi.Services.Identity.Core.Services;
using ToDoApi.Services.Identity.Domain.Entities;

namespace ToDoApi.Services.Identity.Infrastructure.Services;

internal class CryptoService : ICryptoService
{
    private readonly IKeyValueSecretProvider _keyValueSecretProvider;

    public CryptoService(IKeyValueSecretProvider keyValueSecretProvider)
    {
        _keyValueSecretProvider = keyValueSecretProvider;
    }

    public string GenerateSalt()
    {
        return GenerateRandomBase64String(64);
    }

    public string EncryptPassword(string password, string salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var saltBytes = Encoding.UTF8.GetBytes(salt);
        var hashBuffer = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 10000, HashAlgorithmName.SHA256);
        var hashBase64String = Convert.ToBase64String(hashBuffer.GetBytes(128));
        return hashBase64String;
    }

    public string GenerateRefreshToken()
    {
        return GenerateRandomBase64String(32)
            .RemoveByRegex(@"[:\/?#\[\]@!$&'()*+,;=]");
    }

    public bool ValidateUserPassword(User user, string password)
    {
        var encryptedPassword = EncryptPassword(password, user.PasswordSalt!);
        var passwordHash = user.PasswordHash;

        return encryptedPassword.Equals(passwordHash);
    }

    private string GenerateRandomBase64String(int characters)
    {
        var bytes = new byte[characters];
        var provider = RandomNumberGenerator.Create();
        provider.GetBytes(bytes);
        var base64String = Convert.ToBase64String(bytes);
        return base64String;
    }
}