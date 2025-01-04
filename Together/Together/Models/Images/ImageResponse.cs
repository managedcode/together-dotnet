using System.Text.Json.Serialization;

namespace Together.Models.Images;

public class ImageResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("object")]
    public string Object { get; set; } // Use string to handle Literal["list"]

    [JsonPropertyName("data")]
    public List<ImageChoicesData> Data { get; set; }
}