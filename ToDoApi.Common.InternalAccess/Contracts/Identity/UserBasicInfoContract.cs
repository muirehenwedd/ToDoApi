using System;

namespace ToDoApi.Common.InternalAccess.Contracts.Identity;

public sealed class UserBasicInfo
{
    public Guid UserId { get; set; }
    public string Login { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}