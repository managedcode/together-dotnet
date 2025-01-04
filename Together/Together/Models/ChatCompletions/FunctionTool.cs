using System.Text.Json.Serialization;

namespace Together.Models.ChatCompletions;

public class FunctionTool
{
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("parameters")]
    public Dictionary<string, object> Parameters { get; set; }
}