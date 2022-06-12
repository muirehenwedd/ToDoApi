using System;

namespace ToDoApi.Services.Identity.Core.Services.Models;

public class AccessTokenData
{
    public string AccessToken { get; set; }
    public DateTime ExpiresAt { get; set; }
}