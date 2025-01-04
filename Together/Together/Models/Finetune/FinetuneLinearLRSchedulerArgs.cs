using System.Text.Json.Serialization;

namespace Together.Models.Finetune;

public class FinetuneLinearLRSchedulerArgs
{
    [JsonPropertyName("min_lr_ratio")]
    public float? MinLrRatio { get; set; } = 0.0f;
}