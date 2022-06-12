using System;
using ToDoApi.Common.Core.Context;

namespace ToDoApi.Common.Infrastructure.Context;

public class IdentityContext : IIdentityContext
{
    public Guid UserId { get; set; }
}