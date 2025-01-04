namespace Together.Models.Finetune;

public class FinetuneRequest
{
    public string TrainingFile { get; set; }
    public string ValidationFile { get; set; }
    public string Model { get; set; }
    public int NEpochs { get; set; }
    public float LearningRate { get; set; }
    public FinetuneLRScheduler LrScheduler { get; set; }
    public float WarmupRatio { get; set; }
    public float MaxGradNorm { get; set; }
    public float WeightDecay { get; set; }
    public int? NCheckpoints { get; set; }
    public int? NEvals { get; set; }
    public int? BatchSize { get; set; }
    public string Suffix { get; set; }
    public string WandbKey { get; set; }
    public string WandbBaseUrl { get; set; }
    public string WandbProjectName { get; set; }
    public string WandbName { get; set; }
    public string TrainingType { get; set; } // Use object to handle both FullTrainingType and LoRATrainingType
    public string TrainOnInputs { get; set; } = "auto";
}