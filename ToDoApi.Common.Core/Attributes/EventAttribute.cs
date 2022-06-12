using System;

namespace ToDoApi.Common.Core.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class EventAttribute : Attribute
{
    public EventAttribute(string key)
    {
        Key = key;
    }

    public string Key { get; set; }
}