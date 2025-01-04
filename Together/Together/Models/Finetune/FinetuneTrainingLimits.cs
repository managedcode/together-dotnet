using System.Text.Json.Serialization;

namespace Together.Models.Finetune;

public class FinetuneTrainingLimits
{
    [JsonPropertyName("max_num_epochs")]
    public int MaxNumEpochs { get; set; }

    [JsonPropertyName("max_learning_rate")]
    public float MaxLearningRate { get; set; }

    [JsonPropertyName("min_learning_rate")]
    public float MinLearningRate { get; set; }

    [JsonPropertyName("full_training")]
    public FinetuneFullTrainingLimits FullTraining { get; set; }

    [JsonPropertyName("lora_training")]
    public FinetuneLoraTrainingLimits LoraTraining { get; set; }
}