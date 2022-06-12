using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ToDoApi.Common.Core.Attributes;
using ToDoApi.Common.Infrastructure.JSON;
using ToDoApi.Common.Infrastructure.Validation.Attributes;

namespace ToDoApi.Common.Infrastructure.Validation.Internal;

public class PayloadValidator : IPayloadValidator
{
    public ValidationResult<T> Validate<T>(IDictionary<string, object> payload)
    {
        var result = typeof(T)
            .GetProperties()
            .ToList()
            .Select(info =>
            {
                var payloadPropertyAttribute = info.GetCustomAttribute<PayloadPropertyAttribute>();
                var propertyResult =
                    new ValidationResult<T>.Property(payloadPropertyAttribute!.Name, info.PropertyType.Name);
                var requiredAttribute = info.GetCustomAttribute<RequiredAttribute>();

                if (!payload.ContainsKey(payloadPropertyAttribute.Name))
                {
                    if (requiredAttribute is not null)
                        propertyResult.ValidationErrors
                            .Add(ValidationErrorCode.MissingRequired.ToValidationErrorCode());

                    return propertyResult;
                }

                var payloadValue = payload[payloadPropertyAttribute.Name];

                if (requiredAttribute is not null && payloadValue == null)
                {
                    propertyResult.ValidationErrors
                        .Add(ValidationErrorCode.MissingRequiredValue.ToValidationErrorCode());

                    return propertyResult;
                }

                info
                    .GetCustomAttributes<BaseValidationAttribute>()
                    .Where(attr => !attr.Validate(payloadValue))
                    .Select(attr => attr.ErrorCode.ToValidationErrorCode())
                    .ToList()
                    .ForEach(error => propertyResult.ValidationErrors.Add(error));

                return propertyResult;
            })
            .Where(property => property.ValidationErrors.Any())
            .Aggregate(new ValidationResult<T>(), (result, property) =>
            {
                result.Properties.Add(property);
                return result;
            });

        if (payload.FromJsonDictionary<T>() is var valueObject)
            result.Payload = valueObject;

        return result;
    }
}