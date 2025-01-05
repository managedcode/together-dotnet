using System.Text.Json.Serialization;

namespace Together.Models.ChatCompletions;

public class ChatCompletionMessageContent
{
    [JsonPropertyName("type")]
    public ChatCompletionMessageContentType Type { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("image_url")]
    public ChatCompletionMessageContentImageURL ImageUrl { get; set; }
}