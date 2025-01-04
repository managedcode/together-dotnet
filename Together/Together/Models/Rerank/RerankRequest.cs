using System.Text.Json.Nodes;

namespace Together.Models.Rerank;

public class RerankRequest
{
    public string Model { get; set; }
    public string Query { get; set; }
    public List<JsonObject> Documents { get; set; } // Using object to handle both string and Dictionary<string, object>
    public int? TopN { get; set; }
    public bool ReturnDocuments { get; set; } = false;
    public List<string> RankFields { get; set; }
}