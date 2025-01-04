using System.Text.Json.Serialization;

namespace Together.Models.ChatCompletions;

public class ResponseFormat
{
    [JsonPropertyName("type")]
    public ResponseFormatType Type { get; set; }

    [JsonPropertyName("schema")]
    public Dictionary<string, object> Schema { get; set; }

    public Dictionary<string, object> ToDict()
    {
        return new Dictionary<string, object> { { "schema", Schema }, { "type", Type } };
    }
}