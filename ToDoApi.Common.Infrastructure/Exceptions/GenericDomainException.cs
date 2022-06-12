using ToDoApi.Common.Core.Exceptions;
using ToDoApi.Common.Domain;
using ToDoApi.Common.Infrastructure.ErrorHandling;

namespace ToDoApi.Common.Infrastructure.Exceptions;

public class GenericDomainException : BaseException
{
    public GenericDomainException(
        string message,
        BaseDomainException innerException
    ) : base(ErrorScope.Global, message, innerException)
    {
    }
}