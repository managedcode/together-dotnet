namespace Together.Models.Finetune;

public class FinetuneTrainingLimits
{
    public int MaxNumEpochs { get; set; }
    public float MaxLearningRate { get; set; }
    public float MinLearningRate { get; set; }
    public FinetuneFullTrainingLimits FullTraining { get; set; }
    public FinetuneLoraTrainingLimits LoraTraining { get; set; }
}