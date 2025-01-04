namespace Together.Models.Common;

public class LogprobsPart
{
    public List<string> Tokens { get; set; } = new();
    public List<float> TokenLogprobs { get; set; } = new();
}