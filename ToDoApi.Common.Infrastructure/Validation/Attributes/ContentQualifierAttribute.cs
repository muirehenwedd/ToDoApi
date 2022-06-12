using System;
using System.Text.RegularExpressions;

namespace ToDoApi.Common.Infrastructure.Validation.Attributes;

public class ContentQualifierAttribute : BaseValidationAttribute
{
    public override ValidationErrorCode ErrorCode => ValidationErrorCode.InvalidContentQualifier;

    public override bool Validate(object? value)
    {
        if (value is null)
            return true;

        if (value is not string stringValue)
            throw new InvalidCastException();

        return Regex.IsMatch(stringValue, @"^([^\/]+)[\/]([^\/]+)$");
    }
}