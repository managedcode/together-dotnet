namespace Together.Models.Finetune;

public class LoRATrainingType : TrainingType
{
    public LoRATrainingType()
    {
        Type = "Lora";
    }

    public int LoraR { get; set; }
    public int LoraAlpha { get; set; }
    public float LoraDropout { get; set; } = 0.0f;
    public string LoraTrainableModules { get; set; } = "all-linear";
}