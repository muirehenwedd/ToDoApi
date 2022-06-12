using System;

namespace ToDoApi.Common.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class EnvironmentVariableAttribute : Attribute
{
    public EnvironmentVariableAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}