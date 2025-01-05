using System.Text.Json.Serialization;
using Together.Models.Common;

namespace Together.Models.ChatCompletions;

public class ChatCompletionChunk
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]
    public ObjectType Object { get; set; }

    [JsonPropertyName("created")]
    public int? Created { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("choices")]
    public List<ChatCompletionChoicesChunk> Choices { get; set; }

    [JsonPropertyName("finish_reason")]
    public FinishReason FinishReason { get; set; }

    [JsonPropertyName("usage")]
    public UsageData Usage { get; set; }
}