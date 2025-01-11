using Microsoft.Extensions.AI;
using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Together.Models.ChatCompletions;

public static class ChatMessageExtensions
{
    public static ChatCompletionMessage ToChatCompletionMessage(this ChatMessage message)
    {
        return new ChatCompletionMessage
        {
            Role =  message.Role,
            Content = message.Text,
        };
    }

    public static ChatMessage ToChatMessage(this ChatCompletionMessage message)
    {
        return new ChatMessage(message.Role, message.Content)
        {
            AuthorName = message.Role.ToString()
        };
    }

    public static ChatCompletionRequest CreateChatCompletionRequest(
        this IEnumerable<ChatMessage> messages,
        string model,
        bool stream = false)
    {
        return new ChatCompletionRequest
        {
            Messages = messages.Select(m => m.ToChatCompletionMessage()).ToList(),
            Model = model,
            Stream = stream
        };
    }
}

public static class ResponseFormatExtensions
{
    public static ResponseFormat ToResponseFormat(this ChatResponseFormat chatResponseFormat)
    {
        return new ResponseFormat
        {
            Type = chatResponseFormat is ChatResponseFormatText ? ResponseFormatType.JsonObject : ResponseFormatType.JsonSchema,
            Schema = chatResponseFormat is ChatResponseFormatJson jsonFormat 
                ? JsonSerializer.Deserialize<Dictionary<string, object>>(jsonFormat.Schema ?? "{}") 
                : new Dictionary<string, object>()
        };
    }

    public static ChatResponseFormat ToChatResponseFormat(this ResponseFormat responseFormat)
    {
        return responseFormat.Type == ResponseFormatType.JsonObject
            ? ChatResponseFormat.Text
            : ChatResponseFormat.ForJsonSchema(JsonSerializer.Serialize(responseFormat.Schema));
    }
    
    public static StreamingChatMessageContent ToStreamingChatMessageContent(this ChatCompletionChunk chunk)
    {
        var content = chunk.Choices.FirstOrDefault()?.Delta?.Content;
        return content != null ? new StreamingChatMessageContent(AuthorRole.Assistant, content) : null;
    }
}

public static class MessagesExtensions
{
    public static ChatCompletionMessage ToChatCompletionMessage(this ChatMessageContent messageContent)
    {
        ArgumentNullException.ThrowIfNull(messageContent);

        return new ChatCompletionMessage
        {
            Role = new ChatRole(messageContent.Role.ToString()),
            Content = messageContent.Content,
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