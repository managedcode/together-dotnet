using Together.Models.Common;

namespace Together.Models.Rerank;

public class RerankResponse
{
    public string Id { get; set; } 
    public string Object { get; set; }  // Use string to handle Literal["rerank"]
    public string Model { get; set; } 
    public List<RerankChoicesData> Results { get; set; } 
    public UsageData Usage { get; set; } 
}