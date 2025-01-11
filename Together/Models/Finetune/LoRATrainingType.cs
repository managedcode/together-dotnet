using System.Text.Json.Serialization;

namespace Together.Models.Finetune;

public class LoRATrainingType : TrainingType
{
    public LoRATrainingType()
    {
        Type = "Lora";
    }

    [JsonPropertyName("lora_r")]
    public int LoraR { get; set; }

    [JsonPropertyName("lora_alpha")]
    public int LoraAlpha { get; set; }

    [JsonPropertyName("lora_dropout")]
    public float LoraDropout { get; set; } = 0.0f;

    [JsonPropertyName("lora_trainable_modules")]
    public string LoraTrainableModules { get; set; } = "all-linear";
}