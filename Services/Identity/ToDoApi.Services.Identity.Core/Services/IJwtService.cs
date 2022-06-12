using System.Threading.Tasks;
using ToDoApi.Services.Identity.Core.Services.Models;
using ToDoApi.Services.Identity.Domain.Entities;

namespace ToDoApi.Services.Identity.Core.Services;

public interface IJwtService
{
    public Task<AccessTokenData> GenerateAccessToken(User user);
}