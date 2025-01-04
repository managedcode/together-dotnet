using System.Text.Json.Serialization;

namespace Together.Models.Finetune;

public class FinetuneDownloadResult
{
    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("checkpoint_step")]
    public int? CheckpointStep { get; set; }

    [JsonPropertyName("filename")]
    public string Filename { get; set; }

    [JsonPropertyName("size")]
    public int? Size { get; set; }
}