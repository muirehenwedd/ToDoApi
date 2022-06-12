using System;
using ToDoApi.Common.Core.Exceptions;
using ToDoApi.Common.Infrastructure.ErrorHandling;

namespace ToDoApi.Common.Infrastructure.Vault.Exceptions;

public class VaultRuntimeException : BaseException
{
    public VaultRuntimeException(string message) : base(ErrorScope.Vault, message)
    {
    }

    public VaultRuntimeException(string message, Exception innerException) : base(ErrorScope.Vault, message,
        innerException)
    {
    }
}