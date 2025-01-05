using System.Text.Json.Serialization;

namespace Together.Models.Images;

public class ImageChoicesData
{
    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("b64_json")]
    public string B64Json { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}