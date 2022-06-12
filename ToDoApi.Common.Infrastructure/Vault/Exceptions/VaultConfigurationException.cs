using ToDoApi.Common.Core.Exceptions;
using ToDoApi.Common.Infrastructure.ErrorHandling;

namespace ToDoApi.Common.Infrastructure.Vault.Exceptions;

public class VaultConfigurationException : BaseException
{
    public VaultConfigurationException(string message) : base(ErrorScope.Vault, message)
    {
    }
}