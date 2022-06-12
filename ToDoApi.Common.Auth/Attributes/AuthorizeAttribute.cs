using System;

namespace ToDoApi.Common.Auth.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class AuthorizeAttribute : Attribute
{
    public AuthorizeAttribute(TokenUsage tokenUsage = TokenUsage.Authorization)
    {
        TokenUsage = tokenUsage;
    }

    public TokenUsage TokenUsage { get; set; }
}