using System.Text.Json.Serialization;
using Together.Models.Common;

namespace Together.Models.Embeddings;

public class EmbeddingChoicesData
{
    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("object")]
    public ObjectType Object { get; set; }

    [JsonPropertyName("embedding")]
    public List<float> Embedding { get; set; } = new();
}