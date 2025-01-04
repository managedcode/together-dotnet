using System.Text.Json.Serialization;
using Together.Models.Common;

namespace Together.Models.Files;

public class FileDeleteResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]
    public ObjectType Object { get; set; }

    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }
}