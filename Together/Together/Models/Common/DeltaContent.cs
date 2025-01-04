using System.Text.Json.Serialization;

namespace Together.Models.Common;

public class DeltaContent
{
    [JsonPropertyName("content")]
    public string Content { get; set; }
}