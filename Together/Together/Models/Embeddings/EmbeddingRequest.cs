namespace Together.Models.Embeddings;

public class EmbeddingRequest
{
    public object Input { get; set; } // Use object to handle both string and List<string>
    public string Model { get; set; }
}