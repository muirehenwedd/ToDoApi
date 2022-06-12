using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ToDoApi.Common.Auth;
using ToDoApi.Common.Auth.Contracts;
using ToDoApi.Common.Infrastructure.Vault;
using ToDoApi.Services.Identity.Core.Services;
using ToDoApi.Services.Identity.Core.Services.Models;
using ToDoApi.Services.Identity.Domain.Entities;
using ToDoApi.Services.Identity.Infrastructure.Configuration;

namespace ToDoApi.Services.Identity.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IKeyValueSecretProvider _keyValueSecretProvider;

    public JwtService(
        IKeyValueSecretProvider keyValueSecretProvider
    )
    {
        _keyValueSecretProvider = keyValueSecretProvider;
    }

    public async Task<AccessTokenData> GenerateAccessToken(User user)
    {
        var options = await _keyValueSecretProvider.GetAsync<JwtConfigurationOptions>();
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.Secret));
        var handler = new JwtSecurityTokenHandler();

        var userId = user.Id;

        var expirationTimestampUtc = DateTime.UtcNow.AddMinutes(options.TtlMinutes);

        var descriptor = new SecurityTokenDescriptor
        {
            Audience = options.Audience,
            Issuer = options.Issuer,
            Expires = expirationTimestampUtc,
            Claims = new Dictionary<string, object>
            {
                {IdentityClaims.UserId, userId}
            },
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = handler.CreateJwtSecurityToken(descriptor);
        token.Header.Add(HeaderClaims.UsageClaim, TokenUsage.Authorization);

        var tokenString = handler.WriteToken(token);

        return new AccessTokenData
        {
            AccessToken = tokenString,
            ExpiresAt = expirationTimestampUtc
        };
    }
}