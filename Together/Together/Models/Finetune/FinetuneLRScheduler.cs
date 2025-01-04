namespace Together.Models.Finetune;

public class FinetuneLRScheduler
{
    public string LrSchedulerType { get; set; }
    public FinetuneLinearLRSchedulerArgs LrSchedulerArgs { get; set; }
}