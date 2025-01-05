using System.Text.Json.Serialization;

namespace Together.Models.Models;

public class PricingObject
{
    [JsonPropertyName("input")]
    public float? Input { get; set; }

    [JsonPropertyName("output")]
    public float? Output { get; set; }

    [JsonPropertyName("hourly")]
    public float? Hourly { get; set; }

    [JsonPropertyName("base")]
    public float? Base { get; set; }

    [JsonPropertyName("finetune")]
    public float? Finetune { get; set; }
}