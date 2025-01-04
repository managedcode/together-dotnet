namespace Together.Models.Embeddings;

public class EmbeddingResponse
{
    public string Id { get; set; }
    public string Model { get; set; }
    public string Object { get; set; } // Use string to handle Literal["list"]
    public List<EmbeddingChoicesData> Data { get; set; } = new();
}