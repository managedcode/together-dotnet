using System.Text.Json.Nodes;

namespace Together.Models.Rerank;

public class RerankChoicesData
{
    public int Index { get; set; }
    public float RelevanceScore { get; set; }
    public Dictionary<string, JsonObject> Document { get; set; } = new();
}