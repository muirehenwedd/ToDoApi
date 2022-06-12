using Newtonsoft.Json;

namespace ToDoApi.Common.Infrastructure.ErrorHandling.Internals;

internal sealed class ErrorResponse
{
    public ErrorResponse(int errorCode, string message)
    {
        ErrorCode = errorCode;
        Message = message;
    }

    public ErrorResponse(int errorCode, string message, object validationResult)
    {
        ErrorCode = errorCode;
        Message = message;
        ValidationResult = validationResult;
    }

    [JsonProperty("errorCode")]
    public int ErrorCode { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("validationResult")]
    public object? ValidationResult { get; set; }
}