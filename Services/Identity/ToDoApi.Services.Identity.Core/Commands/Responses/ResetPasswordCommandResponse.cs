using Newtonsoft.Json;

namespace ToDoApi.Services.Identity.Core.Commands.Responses;

public class ResetPasswordCommandResponse
{
    [JsonProperty("accessToken")]
    public string AcessToken { get; set; }
}