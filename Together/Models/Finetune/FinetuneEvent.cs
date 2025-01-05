using System.Text.Json.Serialization;
using Together.Models.Common;

namespace Together.Models.Finetune;

public class FinetuneEvent
{
    [JsonPropertyName("object")]
    public ObjectType Object { get; set; }

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; }

    [JsonPropertyName("level")]
    public FinetuneEventLevels? Level { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("type")]
    public FinetuneEventType? Type { get; set; }

    [JsonPropertyName("param_count")]
    public int? ParamCount { get; set; }

    [JsonPropertyName("token_count")]
    public int? TokenCount { get; set; }

    [JsonPropertyName("wandb_url")]
    public string WandbUrl { get; set; }

    [JsonPropertyName("hash")]
    public string Hash { get; set; }
}