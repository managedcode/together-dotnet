using System.Text.Json.Serialization;

namespace Together.Models.ChatCompletions;

public class ToolCalls
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("function")]
    public FunctionCall Function { get; set; }
}