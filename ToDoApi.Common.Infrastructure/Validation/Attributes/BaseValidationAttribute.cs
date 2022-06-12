using System;

namespace ToDoApi.Common.Infrastructure.Validation.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public abstract class BaseValidationAttribute : Attribute
{
    public abstract ValidationErrorCode ErrorCode { get; }

    public abstract bool Validate(object value);
}