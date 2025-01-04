namespace Together.Models.Finetune;

public class FinetuneResponse
{
    public string Id { get; set; }
    public string TrainingFile { get; set; }
    public string ValidationFile { get; set; }
    public string Model { get; set; }
    public string OutputName { get; set; }
    public string AdapterOutputName { get; set; }
    public int? NEpochs { get; set; }
    public int? NCheckpoints { get; set; }
    public int? NEvals { get; set; }
    public int? BatchSize { get; set; }
    public float? LearningRate { get; set; }
    public FinetuneLRScheduler LrScheduler { get; set; }
    public float? WarmupRatio { get; set; }
    public float? MaxGradNorm { get; set; }
    public float? WeightDecay { get; set; }
    public int? EvalSteps { get; set; }
    public TrainingType TrainingType { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
    public FinetuneJobStatus? Status { get; set; }
    public string JobId { get; set; }
    public List<FinetuneEvent> Events { get; set; }
    public int? TokenCount { get; set; }
    public int? ParamCount { get; set; }
    public int? TotalPrice { get; set; }
    public int? TotalSteps { get; set; }
    public int? StepsCompleted { get; set; }
    public int? EpochsCompleted { get; set; }
    public int? EvalsCompleted { get; set; }
    public int? QueueDepth { get; set; }
    public string WandbBaseUrl { get; set; }
    public string WandbProjectName { get; set; }
    public string WandbName { get; set; }
    public string WandbUrl { get; set; }
    public int? TrainingFileNumLines { get; set; }
    public int? TrainingFileSize { get; set; }
    public string TrainOnInputs { get; set; } = "auto";
}