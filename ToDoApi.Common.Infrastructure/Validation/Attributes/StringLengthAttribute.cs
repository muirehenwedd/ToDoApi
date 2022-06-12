using System;

namespace ToDoApi.Common.Infrastructure.Validation.Attributes;

public class StringLengthAttribute : BaseValidationAttribute
{
    private readonly int _length;
    private readonly NumericComparison _numericComparison;

    public StringLengthAttribute(NumericComparison numericComparison, int length)
    {
        _numericComparison = numericComparison;
        _length = length;
    }

    public override ValidationErrorCode ErrorCode => ValidationErrorCode.InvalidLength;

    public override bool Validate(object value)
    {
        if (value is not string stringValue)
            throw new InvalidCastException();

        return Compare(stringValue);
    }

    private bool Compare(string value)
    {
        return _numericComparison switch
        {
            NumericComparison.EqualTo => value.Length == _length,
            NumericComparison.LessThan => value.Length < _length,
            NumericComparison.LessOrEqualTo => value.Length <= _length,
            NumericComparison.GreaterThan => value.Length > _length,
            NumericComparison.GreaterOrEqualTo => value.Length >= _length,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}