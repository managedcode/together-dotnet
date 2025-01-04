using System.Numerics;
using Together.Models.Common;

namespace Together.Models.ChatCompletions;

public class ChatCompletionChoicesChunk
{
    public int? Index { get; set; }
    public float? Logprobs { get; set; }
    public ulong? Seed   { get; set; }
    public FinishReason? FinishReason { get; set; }
    public DeltaContent? Delta { get; set; }
}