using System.Text.Json.Serialization;

namespace Together.Models.Finetune;

public class FinetuneList
{
    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("data")]
    public List<FinetuneResponse> Data { get; set; }
}