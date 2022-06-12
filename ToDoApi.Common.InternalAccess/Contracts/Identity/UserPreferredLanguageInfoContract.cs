using System;

namespace ToDoApi.Common.InternalAccess.Contracts.Identity;

public sealed class UserLanguageInfo
{
    public bool IsPreferredLanguageSet { get; set; }
    public Guid? LanguageId { get; set; }
    public string? CultureCode { get; set; }
    public string? Name { get; set; }
    public string? CultureFlagIcon { get; set; }
}