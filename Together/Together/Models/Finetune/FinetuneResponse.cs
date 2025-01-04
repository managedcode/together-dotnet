using System.Text.Json.Serialization;

namespace Together.Models.Finetune;

public class FinetuneResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("training_file")]
    public string TrainingFile { get; set; }

    [JsonPropertyName("validation_file")]
    public string ValidationFile { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("output_name")]
    public string OutputName { get; set; }

    [JsonPropertyName("adapter_output_name")]
    public string AdapterOutputName { get; set; }

    [JsonPropertyName("n_epochs")]
    public int? NEpochs { get; set; }

    [JsonPropertyName("n_checkpoints")]
    public int? NCheckpoints { get; set; }

    [JsonPropertyName("n_evals")]
    public int? NEvals { get; set; }

    [JsonPropertyName("batch_size")]
    public int? BatchSize { get; set; }

    [JsonPropertyName("learning_rate")]
    public float? LearningRate { get; set; }

    [JsonPropertyName("lr_scheduler")]
    public FinetuneLRScheduler LrScheduler { get; set; }

    [JsonPropertyName("warmup_ratio")]
    public float? WarmupRatio { get; set; }

    [JsonPropertyName("max_grad_norm")]
    public float? MaxGradNorm { get; set; }

    [JsonPropertyName("weight_decay")]
    public float? WeightDecay { get; set; }

    [JsonPropertyName("eval_steps")]
    public int? EvalSteps { get; set; }

    [JsonPropertyName("training_type")]
    public TrainingType TrainingType { get; set; }

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public string UpdatedAt { get; set; }

    [JsonPropertyName("status")]
    public FinetuneJobStatus? Status { get; set; }

    [JsonPropertyName("job_id")]
    public string JobId { get; set; }

    [JsonPropertyName("events")]
    public List<FinetuneEvent> Events { get; set; }

    [JsonPropertyName("token_count")]
    public int? TokenCount { get; set; }

    [JsonPropertyName("param_count")]
    public int? ParamCount { get; set; }

    [JsonPropertyName("total_price")]
    public int? TotalPrice { get; set; }

    [JsonPropertyName("total_steps")]
    public int? TotalSteps { get; set; }

    [JsonPropertyName("steps_completed")]
    public int? StepsCompleted { get; set; }

    [JsonPropertyName("epochs_completed")]
    public int? EpochsCompleted { get; set; }

    [JsonPropertyName("evals_completed")]
    public int? EvalsCompleted { get; set; }

    [JsonPropertyName("queue_depth")]
    public int? QueueDepth { get; set; }

    [JsonPropertyName("wandb_base_url")]
    public string WandbBaseUrl { get; set; }

    [JsonPropertyName("wandb_project_name")]
    public string WandbProjectName { get; set; }

    [JsonPropertyName("wandb_name")]
    public string WandbName { get; set; }

    [JsonPropertyName("wandb_url")]
    public string WandbUrl { get; set; }

    [JsonPropertyName("training_file_num_lines")]
    public int? TrainingFileNumLines { get; set; }

    [JsonPropertyName("training_file_size")]
    public int? TrainingFileSize { get; set; }

    [JsonPropertyName("train_on_inputs")]
    public string TrainOnInputs { get; set; } = "auto";
}