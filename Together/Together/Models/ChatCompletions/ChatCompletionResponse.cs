using System.Text.Json.Serialization;
using Together.Models.Common;

namespace Together.Models.ChatCompletions;

public class ChatCompletionResponse
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
    public List<ChatCompletionChoicesData> Choices { get; set; }

    [JsonPropertyName("prompt")]
    public List<PromptPart> Prompt { get; set; }

    [JsonPropertyName("usage")]
    public UsageData Usage { get; set; }
}