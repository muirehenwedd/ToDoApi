using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ToDoApi.Common.Auth;
using ToDoApi.Common.Auth.Contracts;

namespace ToDoApi.Services.Identity.Infrastructure.Authorization;

public static class Extensions
{
    public static TokenPayload AsTokenPayload(this JwtSecurityToken jwtSecurityToken)
    {
        var jsonPayload = jwtSecurityToken.Payload.SerializeToJson();
        return JsonConvert.DeserializeObject<TokenPayload>(jsonPayload);
    }

    public static ResetPasswordTokenPayload AsResetPasswordTokenPayload(this JwtSecurityToken jwtSecurityToken)
    {
        var jsonPayload = jwtSecurityToken.Payload.SerializeToJson();
        return JsonConvert.DeserializeObject<ResetPasswordTokenPayload>(jsonPayload);
    }

    public static void SetIdentityContext(this HttpContext context, TokenPayload payload)
    {
        context.Items[IdentityItems.TokenPayload] = payload;
    }

    public static void SetIdentityContext(this HttpContext context, ResetPasswordTokenPayload payload)
    {
        context.Items[IdentityItems.TokenPayload] = payload;
    }

    public static void SetAuthorizationTokenUsage(this HttpContext context, TokenUsage usage)
    {
        context.Items[IdentityItems.TokenUsage] = usage;
    }
}