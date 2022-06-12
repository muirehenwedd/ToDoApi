using System;

namespace ToDoApi.Common.Core.Context;

public interface IIdentityContext
{
    public Guid UserId { get; }
}