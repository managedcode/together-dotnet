using System.Text.Json.Serialization;
using Together.Models.Common;

namespace Together.Models.Rerank;

public class RerankResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("results")]
    public List<RerankChoicesData> Results { get; set; }

    [JsonPropertyName("usage")]
    public UsageData Usage { get; set; }
}