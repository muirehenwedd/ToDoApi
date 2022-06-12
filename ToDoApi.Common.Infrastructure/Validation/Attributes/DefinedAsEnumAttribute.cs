using System;

namespace ToDoApi.Common.Infrastructure.Validation.Attributes;

/// <summary>
///     Attribute that validates whether a given integral value exists in a specified enumeration.
/// </summary>
public class DefinedAsEnumAttribute : BaseValidationAttribute
{
    private readonly Type _enumType;

    public DefinedAsEnumAttribute(Type enumType)
    {
        if (!enumType.IsEnum)
            throw new InvalidCastException();

        _enumType = enumType;
    }

    public override ValidationErrorCode ErrorCode => ValidationErrorCode.NotDefinedAsEnum;

    public override bool Validate(object? value)
    {
        return value is null || Enum.IsDefined(_enumType, Convert.ToInt32(value));
    }
}