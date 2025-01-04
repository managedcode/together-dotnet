using System.Text.Json.Serialization;
using Together.Models.Common;

namespace Together.Models.ChatCompletions;

public class ChatCompletionChoicesData
{
    [JsonPropertyName("index")]
    public int? Index { get; set; }

    [JsonPropertyName("logprobs")]
    public LogprobsPart? Logprobs { get; set; }

    [JsonPropertyName("seed")]
    public ulong? Seed { get; set; }

    [JsonPropertyName("finish_reason")]
    public FinishReason? FinishReason { get; set; }

    [JsonPropertyName("message")]
    public ChatCompletionMessage? Message { get; set; }
}