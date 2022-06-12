using System.Collections.Generic;
using Newtonsoft.Json;

namespace ToDoApi.Common.Infrastructure.JSON;

public static class Extensions
{
    public static string ToJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public static object? FromJson(this string jsonString)
    {
        return JsonConvert.DeserializeObject(jsonString);
    }

    public static T? FromJson<T>(this string jsonString)
    {
        return JsonConvert.DeserializeObject<T>(jsonString);
    }

    public static Dictionary<string, object>? ToJsonDictionary(this object obj)
    {
        return obj.ToJson().FromJson<Dictionary<string, object>>();
    }

    public static T? FromJsonDictionary<T>(this IDictionary<string, object> dictionary)
    {
        return dictionary.ToJson().FromJson<T>();
    }
}