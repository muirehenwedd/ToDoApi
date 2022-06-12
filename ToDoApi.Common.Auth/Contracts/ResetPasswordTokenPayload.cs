using System;
using Newtonsoft.Json;

namespace ToDoApi.Common.Auth.Contracts;

public class ResetPasswordTokenPayload
{
    [JsonProperty(IdentityClaims.UserId, Required = Required.Always)]
    public Guid UserId { get; set; }
}