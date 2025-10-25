using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Together.Models.Videos;

public class CreateVideoRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("prompt")]
    public string? Prompt { get; set; }

    [JsonPropertyName("height")]
    public int? Height { get; set; }

    [JsonPropertyName("width")]
    public int? Width { get; set; }

    [JsonPropertyName("seconds")]
    public string? Seconds { get; set; }

    [JsonPropertyName("fps")]
    public int? FramesPerSecond { get; set; }

    [JsonPropertyName("steps")]
    public int? Steps { get; set; }

    [JsonPropertyName("seed")]
    public int? Seed { get; set; }

    [JsonPropertyName("guidance_scale")]
    public float? GuidanceScale { get; set; }

    [JsonPropertyName("output_format")]
    public string? OutputFormat { get; set; }

    [JsonPropertyName("output_quality")]
    public int? OutputQuality { get; set; }

    [JsonPropertyName("negative_prompt")]
    public string? NegativePrompt { get; set; }

    [JsonPropertyName("frame_images")]
    public List<Dictionary<string, object>>? FrameImages { get; set; }

    [JsonPropertyName("reference_images")]
    public List<string>? ReferenceImages { get; set; }

    [JsonExtensionData]
    public Dictionary<string, object?>? AdditionalProperties { get; set; }
}

public class CreateVideoResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}

public class VideoOutputs
{
    [JsonPropertyName("cost")]
    public float Cost { get; set; }

    [JsonPropertyName("video_url")]
    public string VideoUrl { get; set; } = string.Empty;
}

public class VideoError
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
}

public class VideoJob
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("seconds")]
    public string Seconds { get; set; } = string.Empty;

    [JsonPropertyName("size")]
    public string Size { get; set; } = string.Empty;

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("error")]
    public VideoError? Error { get; set; }

    [JsonPropertyName("outputs")]
    public VideoOutputs? Outputs { get; set; }

    [JsonPropertyName("completed_at")]
    public long? CompletedAt { get; set; }
}
