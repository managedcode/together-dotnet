namespace Together.Models.Common;

public class PromptPart
{
    public string Text { get; set; }
    public LogprobsPart? Logprobs { get; set; }
}