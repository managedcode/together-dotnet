using System.Text.Json.Serialization;

namespace Together.Models.Rerank;

public class RerankChoicesData
{
    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("relevance_score")]
    public float RelevanceScore { get; set; }

    [JsonPropertyName("document")]
    public List<string> Document { get; set; } = new();
}