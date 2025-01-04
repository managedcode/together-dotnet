using System.Text.Json.Serialization;

namespace Together.Models.Embeddings;

public class EmbeddingResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("object")]
    public string Object { get; set; } // Use string to handle Literal["list"]

    [JsonPropertyName("data")]
    public List<EmbeddingChoicesData> Data { get; set; } = new();
}