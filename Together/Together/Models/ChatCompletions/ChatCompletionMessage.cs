using Microsoft.Extensions.AI;

namespace Together.Models.ChatCompletions;

public class ChatCompletionMessage
{
    public ChatRole Role { get; set; }
    public string Content { get; set; } // Use object to handle both string and List<ChatCompletionMessageContent>
    public List<ToolCalls>? ToolCalls { get; set; }
}