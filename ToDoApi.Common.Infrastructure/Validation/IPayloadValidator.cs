using System.Collections.Generic;

namespace ToDoApi.Common.Infrastructure.Validation;

public interface IPayloadValidator
{
    public ValidationResult<T> Validate<T>(IDictionary<string, object> payload);
}