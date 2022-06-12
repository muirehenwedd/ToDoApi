using System;

namespace ToDoApi.Common.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ConfigOptionsAttribute : Attribute
{
    public ConfigOptionsAttribute(string sectionName)
    {
        SectionName = sectionName;
    }

    public string SectionName { get; set; }
}