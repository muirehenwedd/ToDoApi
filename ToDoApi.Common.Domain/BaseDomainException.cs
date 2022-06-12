using System;
using System.Reflection;

namespace ToDoApi.Common.Domain;

public abstract class BaseDomainException : Exception
{
    protected BaseDomainException(
        MemberInfo aggregateRoot,
        MemberInfo? domainEntity,
        string message
    ) : base($"{aggregateRoot.Name}.{domainEntity?.Name ?? "Self"}: {message}")
    {
    }
}