using System.Text.Json.Serialization;

namespace Together.Models.Images;

public class ImageRequest
{
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("steps")]
    public int? Steps { get; set; } = 20;

    [JsonPropertyName("seed")]
    public ulong? Seed { get; set; }

    [JsonPropertyName("n")]
    public int? N { get; set; } = 1;

    [JsonPropertyName("height")]
    public int? Height { get; set; } = 1024;

    [JsonPropertyName("width")]
    public int? Width { get; set; } = 1024;

    [JsonPropertyName("negative_prompt")]
    public string? NegativePrompt { get; set; }
    
    [JsonPropertyName("image_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ImageUrl { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("image_loras")]
    public List<ImageLora>? ImageLoras { get; set; }

    [JsonPropertyName("response_format")]
    public string ResponseFormat { get; set; } = "url";
}