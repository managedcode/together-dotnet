using System.Text.Json.Serialization;

namespace Together.Models.Finetune;

public class FinetuneFullTrainingLimits
{
    [JsonPropertyName("max_batch_size")]
    public int MaxBatchSize { get; set; }

    [JsonPropertyName("min_batch_size")]
    public int MinBatchSize { get; set; }
}