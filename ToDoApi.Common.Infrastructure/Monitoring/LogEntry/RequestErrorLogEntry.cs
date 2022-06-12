using Newtonsoft.Json;
using ToDoApi.Common.Core.Context;

namespace ToDoApi.Common.Infrastructure.Monitoring.LogEntry;

internal class RequestErrorLogEntry
{
    [JsonProperty("method")]
    public string Method { get; set; }

    [JsonProperty("uri")]
    public string Uri { get; set; }

    [JsonProperty("statusCode")]
    public int StatusCode { get; set; }

    [JsonProperty("elapsed")]
    public int Elapsed { get; set; }

    [JsonProperty("clientIp")]
    public string ClientIp { get; set; }

    [JsonProperty("userAgent")]
    public string UserAgent { get; set; }

    [JsonProperty("exception")]
    public string Exception { get; set; }

    [JsonProperty("errorCode")]
    public int ErrorCode { get; set; }

    [JsonProperty("errorScope")]
    public string ErrorScope { get; set; }

    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; set; }

    [JsonProperty("stackTrace")]
    public string StackTrace { get; set; }

    [JsonProperty("identity")]
    public IIdentityContext? Identity { get; set; }
}