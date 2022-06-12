using System;

namespace ToDoApi.Services.Identity.Core.Commands.Responses;

public class SignInCommandResponse
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime TokenExpiresAt { get; set; }
    public string RefreshToken { get; set; }
}