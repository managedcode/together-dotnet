using System.Text.Json.Serialization;

namespace Together.Models.Finetune;

public class FinetuneLoraTrainingLimits : FinetuneFullTrainingLimits
{
    [JsonPropertyName("max_rank")]
    public int MaxRank { get; set; }

    [JsonPropertyName("target_modules")]
    public List<string> TargetModules { get; set; }
}