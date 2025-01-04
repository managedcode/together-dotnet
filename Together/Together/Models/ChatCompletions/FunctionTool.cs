namespace Together.Models.ChatCompletions;

public class FunctionTool
{
    public string Description { get; set; } 
    public string Name { get; set; }
    public Dictionary<string, object> Parameters { get; set; } 
}