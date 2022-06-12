using System;

namespace ToDoApi.Services.Identity.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; internal set; }
    public string Token { get; internal set; }
    public DateTime CreatedTimestamp { get; internal set; }
    public DateTime? RevokedTimestamp { get; internal set; }
    public User User { get; internal set; }

    public bool IsRevoked => RevokedTimestamp is not null;
}