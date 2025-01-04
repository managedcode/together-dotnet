using Microsoft.Extensions.AI;

namespace Together.Models.Common;

public class BaseModel
{
    // Implementing a basic model configuration allowing extra fields
    // as there is no direct equivalent in C# to Pydantic's `ConfigDict`
    public AdditionalPropertiesDictionary ExtraFields { get; set; } = new();
}