using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ToDoApi.Common.Core.Attributes;
using ToDoApi.Common.Infrastructure.Exceptions.API;

namespace ToDoApi.Common.Infrastructure.CQRS.Internals;

internal static class Validation
{
    internal static void TryValidatePayload<T>(this IDictionary<string, object> dictionary)
    {
        var type = typeof(T);

        type.GetProperties().ToList().ForEach(property =>
        {
            var payloadPropertyAttribute = property.GetCustomAttribute<PayloadPropertyAttribute>();
            var requiredAttribute = property.GetCustomAttribute<RequiredAttribute>();

            if (payloadPropertyAttribute is null) return;

            if (requiredAttribute is null) return;
            if (!dictionary.ContainsKey(payloadPropertyAttribute.Name))
                throw new BadRequestException(
                    $"Request payload is invalid: missing required property '{payloadPropertyAttribute.Name}' of type '{property.PropertyType}'");
        });
    }
}