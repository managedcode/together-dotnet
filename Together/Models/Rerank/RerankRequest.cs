using System.Text.Json.Serialization;
using System.Text.Json.Nodes;

namespace Together.Models.Rerank;

public class RerankRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("query")]
    public string Query { get; set; }

    [JsonPropertyName("documents")]
    public List<string> Documents { get; set; }

    [JsonPropertyName("top_n")]
    public int? TopN { get; set; }

    [JsonPropertyName("return_documents")]
    public bool ReturnDocuments { get; set; } = false;

    [JsonPropertyName("rank_fields")]
    public List<string> RankFields { get; set; }
}