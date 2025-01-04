using Together.Models.Common;

namespace Together.Models.Embeddings;

public class EmbeddingChoicesData
{
    public int Index { get; set; }
    public ObjectType Object { get; set; }
    public List<float> Embedding { get; set; } = new();
}