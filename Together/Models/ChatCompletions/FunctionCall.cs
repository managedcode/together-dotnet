using System.Text.Json.Serialization;

namespace Together.Models.ChatCompletions;

public class FunctionCall
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("arguments")]
    public string Arguments { get; set; }
}