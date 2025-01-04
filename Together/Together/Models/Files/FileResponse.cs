using System.Text.Json.Serialization;
using Together.Models.Common;

namespace Together.Models.Files;

public class FileResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]
    public ObjectType Object { get; set; }

    [JsonPropertyName("created_at")]
    public int? CreatedAt { get; set; }

    [JsonPropertyName("type")]
    public FileType? Type { get; set; }

    [JsonPropertyName("purpose")]
    public FilePurpose? Purpose { get; set; }

    [JsonPropertyName("filename")]
    public string Filename { get; set; }

    [JsonPropertyName("bytes")]
    public int? Bytes { get; set; }

    [JsonPropertyName("LineCount")]
    public int? LineCount { get; set; }

    [JsonPropertyName("Processed")]
    public bool? Processed { get; set; }
}