using System.Text.Json.Serialization;

namespace Together.Models.Common;

public class PromptPart
{
    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("logprobs")]
    public LogprobsPart? Logprobs { get; set; }
}