namespace Together.Models.Finetune;

public class FinetuneLoraTrainingLimits : FinetuneFullTrainingLimits
{
    public int MaxRank { get; set; }
    public List<string> TargetModules { get; set; }
}