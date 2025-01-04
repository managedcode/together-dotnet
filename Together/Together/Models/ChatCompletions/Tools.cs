namespace Together.Models.ChatCompletions;

public class Tools
{
    public string Type { get; set; }
    public FunctionTool Function { get; set; }
}