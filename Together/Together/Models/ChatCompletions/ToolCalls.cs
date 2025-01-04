namespace Together.Models.ChatCompletions;

public class ToolCalls
{
    public string Id { get; set; } 
    public string Type { get; set; } 
    public FunctionCall Function { get; set; } 
}