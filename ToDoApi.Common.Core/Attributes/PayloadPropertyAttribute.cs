using System;

namespace ToDoApi.Common.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PayloadPropertyAttribute : Attribute
{
    public PayloadPropertyAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}