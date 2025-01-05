using System.Text.Json.Serialization;

namespace Together.Models.Files;

public class FileObject
{
    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("filename")]
    public string Filename { get; set; }

    [JsonPropertyName("size")]
    public int? Size { get; set; }
}