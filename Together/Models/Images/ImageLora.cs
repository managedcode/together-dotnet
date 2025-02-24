using System.Text.Json.Serialization;

namespace Together.Models.Images;

public class ImageLora
{
    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("scale")]
    public float Scale { get; set; }
}
