using System.Text.Json.Serialization;

namespace Together.Models.Completions;

public class CompletionRequest
{
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }

    [JsonPropertyName("stop")]
    public List<string> Stop { get; set; }

    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }

    [JsonPropertyName("top_p")]
    public float? TopP { get; set; }

    [JsonPropertyName("top_k")]
    public int? TopK { get; set; }

    [JsonPropertyName("repetition_penalty")]
    public float? RepetitionPenalty { get; set; }

    [JsonPropertyName("presence_penalty")]
    public float? PresencePenalty { get; set; }

    [JsonPropertyName("frequency_penalty")]
    public float? FrequencyPenalty { get; set; }

    [JsonPropertyName("min_p")]
    public float? MinP { get; set; }

    [JsonPropertyName("logit_bias")]
    public Dictionary<string, float> LogitBias { get; set; }

    [JsonPropertyName("seed")]
    public ulong? Seed { get; set; }

    [JsonPropertyName("stream")]
    public bool Stream { get; set; } = false;

    [JsonPropertyName("logprobs")]
    public int? Logprobs { get; set; }

    [JsonPropertyName("echo")]
    public bool? Echo { get; set; }

    [JsonPropertyName("n")]
    public int? N { get; set; }

    [JsonPropertyName("safety_model")]
    public string SafetyModel { get; set; }

    public void VerifyParameters()
    {
        if (RepetitionPenalty.HasValue && (PresencePenalty.HasValue || FrequencyPenalty.HasValue))
        {
            throw new ArgumentException("RepetitionPenalty is not advisable to be used alongside PresencePenalty or FrequencyPenalty");
        }
    }
}