using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ToDoApi.Common.Infrastructure.Validation;

public sealed class ValidationResult<T>
{
    public ValidationResult()
    {
        Properties = new List<Property>();
    }

    [JsonIgnore]
    public T? Payload { get; set; }

    [JsonProperty("isValid")]
    public bool IsValid => Payload is not null && !Properties.Any();

    [JsonProperty("properties")]
    public ICollection<Property> Properties { get; }

    public sealed class Property
    {
        public Property(string name, string type)
        {
            Name = name;
            Type = type;
            ValidationErrors = new List<string>();
        }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("validationErrors")]
        public ICollection<string> ValidationErrors { get; set; }
    }
}