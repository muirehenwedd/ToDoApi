namespace ToDoApi.Common.Infrastructure.Validation;

public enum ValidationErrorCode
{
    Generic = 0,
    MissingRequired = 1,
    MissingRequiredValue = 2,
    InvalidLength = 3,
    RegexMatch = 4,
    EmptyStringValue = 5,
    InvalidContentQualifier = 6,
    NotDefinedAsEnum = 7,
    NotInRange = 8
}

public static class ValidationErrorCodesExtensions
{
    public static string ToValidationErrorCode(this ValidationErrorCode code)
    {
        return $"VEC{((int) code).ToString().PadLeft(5, '0')}";
    }
}