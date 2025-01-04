using System.Text.Json.Serialization;

namespace Together.Models.ChatCompletions;

public class Tools
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("function")]
    public FunctionTool Function { get; set; }
}