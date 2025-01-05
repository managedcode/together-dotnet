using System.Text.Json.Serialization;

namespace Together.Models.Finetune;

public class TrainingType
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}