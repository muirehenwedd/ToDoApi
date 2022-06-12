using System;

namespace ToDoApi.Common.Infrastructure.Validation.Attributes;

public class InRangeAttribute : BaseValidationAttribute
{
    private int? _maxValue;

    private int? _minValue;
    public override ValidationErrorCode ErrorCode => ValidationErrorCode.NotInRange;

    public int MinValue
    {
        set => _minValue = value;
        get => _minValue.Value;
    }

    public int MaxValue
    {
        set => _maxValue = value;
        get => _maxValue.Value;
    }

    public override bool Validate(object? value)
    {
        if (value is null)
            return true;

        switch (value)
        {
            case string stringValue when int.TryParse(stringValue, out var intFromStringValue):
                return (_minValue ?? int.MinValue) <= intFromStringValue &&
                       intFromStringValue <= (_maxValue ?? int.MaxValue);

            case string stringValue when float.TryParse(stringValue, out var floatFromStringValue):
                return ((float?) _minValue ?? float.MinValue) <= floatFromStringValue &&
                       floatFromStringValue <= ((float?) _maxValue ?? float.MaxValue);

            case long longValue:
                var intValue = Convert.ToInt32(longValue);
                return (_minValue ?? int.MinValue) <= intValue && intValue <= (_maxValue ?? int.MaxValue);

            case double doubleValue:
                return ((double?) _minValue ?? double.MinValue) <= doubleValue &&
                       doubleValue <= ((double?) _maxValue ?? double.MaxValue);

            default:
                throw new InvalidCastException();
        }
    }
}