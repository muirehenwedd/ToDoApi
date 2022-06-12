using System;

namespace ToDoApi.Services.Identity.Core.Queries.Responses;

public class FreshTokenAcquireQueryResponse
{
    public string Token { get; set; }
    public DateTime TokenExpiresAt { get; set; }
}