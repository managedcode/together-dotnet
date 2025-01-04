namespace Together.Models.ChatCompletions;

public class ToolChoice
{
    public string Type { get; set; }
    public FunctionToolChoice Function { get; set; }
}