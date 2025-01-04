using System.Text.Json.Serialization;

namespace Together.Models.ChatCompletions;

public class ToolChoice
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("function")]
    public FunctionToolChoice Function { get; set; }
}