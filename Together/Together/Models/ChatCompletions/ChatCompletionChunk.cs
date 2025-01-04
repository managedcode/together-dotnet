using Together.Models.Common;

namespace Together.Models.ChatCompletions;

public class ChatCompletionChunk
{
    public string Id { get; set; } 
    public ObjectType Object { get; set; } 
    public int? Created { get; set; }
    public string Model { get; set; }
    public List<ChatCompletionChoicesChunk> Choices { get; set; } 
    public FinishReason FinishReason { get; set; } 
    public UsageData Usage { get; set; } 
}