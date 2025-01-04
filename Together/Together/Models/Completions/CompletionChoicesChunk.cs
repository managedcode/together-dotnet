using Together.Models.Common;

namespace Together.Models.Completions;

public class CompletionChoicesChunk
{
    public int? Index { get; set; }
    public float? Logprobs { get; set; }
    public int? Seed { get; set; }
    public FinishReason? FinishReason { get; set; }
    public DeltaContent Delta { get; set; }
}