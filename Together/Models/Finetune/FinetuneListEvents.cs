using System.Text.Json.Serialization;

namespace Together.Models.Finetune;

public class FinetuneListEvents
{
    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("data")]
    public List<FinetuneEvent> Data { get; set; }
}