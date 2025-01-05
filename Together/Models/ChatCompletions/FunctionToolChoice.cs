using System.Text.Json.Serialization;

namespace Together.Models.ChatCompletions;

public class FunctionToolChoice
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}