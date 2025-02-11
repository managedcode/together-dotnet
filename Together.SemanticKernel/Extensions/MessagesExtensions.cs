using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Together.Models.ChatCompletions;

namespace Together.SemanticKernel.Extensions;

public static class MessagesExtensions
{
    public static ChatCompletionMessage ToChatCompletionMessage(this ChatMessageContent messageContent)
    {
        ArgumentNullException.ThrowIfNull(messageContent);

        return new ChatCompletionMessage
        {
            Role = new ChatRole(messageContent.Role.ToString()),
            Content = messageContent.Content
        };
    }

    public static IEnumerable<ChatCompletionMessage> ToChatCompletionMessages(this ChatHistory history)
    {
        ArgumentNullException.ThrowIfNull(history);

        foreach (var item in history)
        {
            yield return item.ToChatCompletionMessage();
        }
    }
}