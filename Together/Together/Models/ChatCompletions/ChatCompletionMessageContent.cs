namespace Together.Models.ChatCompletions;

public class ChatCompletionMessageContent
{
    public ChatCompletionMessageContentType Type { get; set; }
    public string Text { get; set; }
    public ChatCompletionMessageContentImageURL ImageUrl { get; set; }
}