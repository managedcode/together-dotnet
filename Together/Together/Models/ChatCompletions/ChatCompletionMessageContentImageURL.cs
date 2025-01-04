using System.Text.Json.Serialization;

namespace Together.Models.ChatCompletions;

public class ChatCompletionMessageContentImageURL
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
}