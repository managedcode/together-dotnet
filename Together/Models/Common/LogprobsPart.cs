using System.Text.Json.Serialization;

namespace Together.Models.Common;

public class LogprobsPart
{
    [JsonPropertyName("tokens")]
    public List<string> Tokens { get; set; } = new();

    [JsonPropertyName("token_logprobs")]
    public List<float> TokenLogprobs { get; set; } = new();
}