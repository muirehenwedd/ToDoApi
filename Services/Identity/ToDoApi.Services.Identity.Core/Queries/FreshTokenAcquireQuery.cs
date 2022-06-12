using System;
using ToDoApi.Common.Core.Attributes;
using ToDoApi.Common.Core.CQRS.Queries;

namespace ToDoApi.Services.Identity.Core.Queries;

public class FreshTokenAcquireQuery : IQuery
{
    [Required]
    [PayloadProperty("userId")]
    public Guid UserId { get; set; }

    [Required]
    [PayloadProperty("refreshToken")]
    public string RefreshToken { get; set; }
}