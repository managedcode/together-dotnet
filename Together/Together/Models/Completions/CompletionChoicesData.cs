using Together.Models.Common;

namespace Together.Models.Completions;

public class CompletionChoicesData
{
    public int Index { get; set; }
    public LogprobsPart Logprobs { get; set; }
    public int? Seed { get; set; }
    public FinishReason FinishReason { get; set; }
    public string Text { get; set; }
}