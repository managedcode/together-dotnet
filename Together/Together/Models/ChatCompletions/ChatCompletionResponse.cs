using Together.Models.Common;

namespace Together.Models.ChatCompletions;

public class ChatCompletionResponse
{
    public string Id { get; set; } 
    public ObjectType Object { get; set; }
    public int? Created { get; set; } 
    public string Model { get; set; } 
    public List<ChatCompletionChoicesData> Choices { get; set; } 
    public List<PromptPart> Prompt { get; set; }  // Use List<object> to handle List<PromptPart> and List<None>
    public UsageData Usage { get; set; } 
}