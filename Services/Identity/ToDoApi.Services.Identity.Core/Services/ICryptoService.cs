using System.Threading.Tasks;
using ToDoApi.Services.Identity.Domain.Entities;

namespace ToDoApi.Services.Identity.Core.Services;

public interface ICryptoService
{
    public string GenerateSalt();
    public string EncryptPassword(string password, string salt);
    public string GenerateRefreshToken();
    public bool ValidateUserPassword(User user, string password);
}