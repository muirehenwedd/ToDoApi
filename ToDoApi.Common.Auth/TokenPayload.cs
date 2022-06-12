using System;
using Newtonsoft.Json;
using ToDoApi.Common.Auth.Contracts;

namespace ToDoApi.Common.Auth;

public sealed class TokenPayload
{
    [JsonProperty(IdentityClaims.UserId, Required = Required.Always)]
    public Guid UserId { get; set; }
}