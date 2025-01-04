using Together.Models.Common;

namespace Together.Models.ChatCompletions;

public class ChatCompletionChoicesData
{
    public int? Index { get; set; }
    public LogprobsPart? Logprobs { get; set; }
    public int? Seed { get; set; } 
    public FinishReason? FinishReason { get; set; }
    public ChatCompletionMessage? Message { get; set; } 
}