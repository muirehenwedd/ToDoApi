using System;
using System.Threading.Tasks;
using ToDoApi.Common.InternalAccess.Contracts.Identity;

namespace ToDoApi.Common.InternalAccess.Accessors;

public interface IIdentityServiceAccessor
{
    public Task<UserBasicInfo> GetUserBasicInfo(Guid userId);
}