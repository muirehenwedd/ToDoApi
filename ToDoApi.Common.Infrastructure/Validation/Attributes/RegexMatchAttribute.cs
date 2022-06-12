using System;
using System.Text.RegularExpressions;

namespace ToDoApi.Common.Infrastructure.Validation.Attributes;

public class RegexMatchAttribute : BaseValidationAttribute
{
    private readonly string _pattern;

    public RegexMatchAttribute(string pattern)
    {
        _pattern = pattern;
    }

    public override ValidationErrorCode ErrorCode => ValidationErrorCode.RegexMatch;

    public override bool Validate(object value)
    {
        if (value is not string stringValue) throw new InvalidCastException();

        return Regex.IsMatch(stringValue, _pattern);
    }
}