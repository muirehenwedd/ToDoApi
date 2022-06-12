using System;

namespace ToDoApi.Common.Infrastructure.Validation.Attributes;

public class NotEmptyStringAttribute : BaseValidationAttribute
{
    public override ValidationErrorCode ErrorCode => ValidationErrorCode.EmptyStringValue;

    public override bool Validate(object? value)
    {
        if (value is null)
            return true;

        if (value is not string stringValue)
            throw new InvalidCastException();

        return stringValue.Length != 0;
    }
}