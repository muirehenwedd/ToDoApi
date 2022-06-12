using System;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using ToDoApi.Common.Auth;
using ToDoApi.Common.Auth.Contracts;
using ToDoApi.Common.Infrastructure.Monitoring;
using ToDoApi.Common.Infrastructure.Vault;
using ToDoApi.Services.Identity.Infrastructure.Configuration;
using ToDoApi.Services.Identity.Infrastructure.Exceptions;

namespace ToDoApi.Services.Identity.Infrastructure.Authorization;

public class AuthorizationMiddleware
{
    private readonly IKeyValueSecretProvider _keyValueSecretProvider;
    private readonly ILogger _logger;
    private readonly RequestDelegate _requestDelegate;

    public AuthorizationMiddleware(
        RequestDelegate requestDelegate,
        IKeyValueSecretProvider keyValueSecretProvider,
        ILogger logger
    )
    {
        _requestDelegate = requestDelegate;
        _keyValueSecretProvider = keyValueSecretProvider;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers[HttpHeaders.Authorization].FirstOrDefault();
        await AttachIdentityToContext(context, token);
        await _requestDelegate.Invoke(context);
    }

    private async Task AttachIdentityToContext(HttpContext context, string? token)
    {
        if (string.IsNullOrEmpty(token))
            return;

        try
        {
            var (tokenUsage, jwtToken) = await ValidateToken(token);

            context.SetAuthorizationTokenUsage(tokenUsage);

            switch (tokenUsage)
            {
                case TokenUsage.Authorization:
                    var payload = jwtToken.AsTokenPayload();
                    context.SetIdentityContext(payload);
                    break;

                case TokenUsage.Unset:
                default: return;
            }
        }
        catch (Exception exception)
        {
            throw new UnauthorizedException(exception);
        }
    }

    private async Task<(TokenUsage, JwtSecurityToken)> ValidateToken(string token)
    {
        var jwtOptions = await _keyValueSecretProvider.GetAsync<JwtConfigurationOptions>();
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Secret));
        var handler = new JwtSecurityTokenHandler();

        handler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        if (validatedToken is not JwtSecurityToken jwtToken)
            throw new NullReferenceException("Validated JWT token is null.");

        try
        {
            var tokenUsageClaim = jwtToken.Header[HeaderClaims.UsageClaim];
            var tokenUsage = (TokenUsage) tokenUsageClaim;
            return (tokenUsage, jwtToken);
        }
        catch (Exception exception)
        {
            throw new InvalidEnumArgumentException("Unable to validate token usage.", exception);
        }
    }
}