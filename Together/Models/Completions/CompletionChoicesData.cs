using System.Text.Json.Serialization;
using Together.Models.Common;

namespace Together.Models.Completions;

public class CompletionChoicesData
{
    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("logprobs")]
    public LogprobsPart Logprobs { get; set; }

    [JsonPropertyName("seed")]
    public ulong? Seed { get; set; }

    [JsonPropertyName("finish_reason")]
    public FinishReason FinishReason { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }
}