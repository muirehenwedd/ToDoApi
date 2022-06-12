namespace ToDoApi.Common.Auth.Contracts;

public static class HttpHeaders
{
    public static string Authorization => "Authorization";
    public static string ApiKey => "x-CS-API-key";
    public static string LastModified => "Last-Modified";
    public static string IfModifiedSince => "If-Modified-Since";
}