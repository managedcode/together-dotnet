using System.Text.Json.Serialization;

namespace Together.Models.Finetune;

public class FinetuneRequest
{
    [JsonPropertyName("training_file")]
    public string TrainingFile { get; set; }

    [JsonPropertyName("validation_file")]
    public string ValidationFile { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("n_epochs")]
    public int NEpochs { get; set; }

    [JsonPropertyName("learning_rate")]
    public float LearningRate { get; set; }

    [JsonPropertyName("lr_scheduler")]
    public FinetuneLRScheduler LrScheduler { get; set; }

    [JsonPropertyName("warmup_ratio")]
    public float WarmupRatio { get; set; }

    [JsonPropertyName("max_grad_norm")]
    public float MaxGradNorm { get; set; }

    [JsonPropertyName("weight_decay")]
    public float WeightDecay { get; set; }

    [JsonPropertyName("n_checkpoints")]
    public int? NCheckpoints { get; set; }

    [JsonPropertyName("n_evals")]
    public int? NEvals { get; set; }

    [JsonPropertyName("batch_size")]
    public int? BatchSize { get; set; }

    [JsonPropertyName("suffix")]
    public string Suffix { get; set; }

    [JsonPropertyName("wandb_key")]
    public string WandbKey { get; set; }

    [JsonPropertyName("wandb_base_url")]
    public string WandbBaseUrl { get; set; }

    [JsonPropertyName("wandb_project_name")]
    public string WandbProjectName { get; set; }

    [JsonPropertyName("wandb_name")]
    public string WandbName { get; set; }

    [JsonPropertyName("training_type")]
    public string TrainingType { get; set; }

    [JsonPropertyName("train_on_inputs")]
    public string TrainOnInputs { get; set; } = "auto";
}