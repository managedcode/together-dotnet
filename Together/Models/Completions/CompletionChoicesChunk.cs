using System.Text.Json.Serialization;
using Together.Models.Common;

namespace Together.Models.Completions;

public class CompletionChoicesChunk
{
    [JsonPropertyName("index")]
    public int? Index { get; set; }

    [JsonPropertyName("logprobs")]
    public float? Logprobs { get; set; }

    [JsonPropertyName("seed")]
    public ulong? Seed { get; set; }

    [JsonPropertyName("finish_reason")]
    public FinishReason? FinishReason { get; set; }

    [JsonPropertyName("delta")]
    public DeltaContent Delta { get; set; }
}