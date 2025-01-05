using System.Text.Json.Serialization;

namespace Together.Models.Finetune;

public class FinetuneLRScheduler
{
    [JsonPropertyName("lr_scheduler_type")]
    public string LrSchedulerType { get; set; }

    [JsonPropertyName("lr_scheduler_args")]
    public FinetuneLinearLRSchedulerArgs LrSchedulerArgs { get; set; }
}