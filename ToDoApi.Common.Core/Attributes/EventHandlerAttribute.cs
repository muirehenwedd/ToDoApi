using System;

namespace ToDoApi.Common.Core.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class EventHandlerAttribute : Attribute
{
    public EventHandlerAttribute(Type eventType)
    {
        EventType = eventType;
    }

    public Type EventType { get; set; }
}