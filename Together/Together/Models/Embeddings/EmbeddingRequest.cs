using System.Text.Json.Serialization;

namespace Together.Models.Embeddings;

public class EmbeddingRequest
{
    [JsonPropertyName("input")]
    public object Input { get; set; } // Use object to handle both string and List<string>

    [JsonPropertyName("model")]
    public string Model { get; set; }
}