namespace Together.Models.ChatCompletions;

public class FunctionCall
{
    public string Name { get; set; } 
    public string Arguments { get; set; } 
}