namespace Together.Models.Completions;

public class CompletionRequest
{
    public string Prompt { get; set; }
    public string Model { get; set; }
    public int? MaxTokens { get; set; }
    public List<string> Stop { get; set; }
    public float? Temperature { get; set; }
    public float? TopP { get; set; }
    public int? TopK { get; set; }
    public float? RepetitionPenalty { get; set; }
    public float? PresencePenalty { get; set; }
    public float? FrequencyPenalty { get; set; }
    public float? MinP { get; set; }
    public Dictionary<string, float> LogitBias { get; set; }
    public int? Seed { get; set; }
    public bool Stream { get; set; } = false;
    public int? Logprobs { get; set; }
    public bool? Echo { get; set; }
    public int? N { get; set; }
    public string SafetyModel { get; set; }

    public void VerifyParameters()
    {
        if (RepetitionPenalty.HasValue && (PresencePenalty.HasValue || FrequencyPenalty.HasValue))
        {
            throw new ArgumentException("RepetitionPenalty is not advisable to be used alongside PresencePenalty or FrequencyPenalty");
        }
    }
}