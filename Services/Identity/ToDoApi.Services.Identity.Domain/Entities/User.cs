using System;
using System.Collections.Generic;
using ToDoApi.Common.Domain;

namespace ToDoApi.Services.Identity.Domain.Entities;

public class User : AggregateRoot
{
    public User()
    {
        RefreshTokens = new HashSet<RefreshToken>();
    }

    public string Login { get; set; }
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
    public DateTime CreationTimestamp { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; }

    public void AddRefreshToken(string token)
    {
        RefreshTokens.Add(new RefreshToken
        {
            Token = token,
            CreatedTimestamp = DateTime.UtcNow,
            RevokedTimestamp = null
        });
    }

    public void DeleteRefreshToken(RefreshToken token)
    {
        RefreshTokens.Remove(token);
    }

    public void RevokeToken(RefreshToken token)
    {
        token.RevokedTimestamp = DateTime.UtcNow;
    }
}