using System.Text.Json.Serialization;
using Microsoft.Extensions.AI;

namespace Together.Models.ChatCompletions;

public class ChatCompletionMessage
{
    [JsonPropertyName("role")]
    public ChatRole Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("tool_calls")]
    public List<ToolCalls> ToolCalls { get; set; }
}