using System.Text.Json;
using Microsoft.Extensions.AI;
using Together.Models.ChatCompletions;

namespace Together.SemanticKernel.Extensions;

public static class ChatMessageExtensions
{
    public static ChatCompletionMessage ToChatCompletionMessage(this ChatMessage message)
    {
        return new ChatCompletionMessage
        {
            Role = message.Role,
            Content = message.Text
        };
    }

    public static ChatMessage ToChatMessage(this ChatCompletionMessage message)
    {
        return new ChatMessage(message.Role, message.Content)
        {
            AuthorName = message.Role.ToString()
        };
    }

    public static ChatCompletionRequest CreateChatCompletionRequest(this IEnumerable<ChatMessage> messages, string model, bool stream = false)
    {
        return new ChatCompletionRequest
        {
            Messages = messages.Select(m => m.ToChatCompletionMessage())
                .ToList(),
            Model = model,
            Stream = stream
        };
    }

    private static string GetRole(JsonElement message)
    {
        if (message.TryGetProperty("role", out var roleElement))
        {
            return roleElement.ValueKind == JsonValueKind.String ? roleElement.GetString() ?? string.Empty : string.Empty;
        }

        return string.Empty;
    }

    private static string GetContent(JsonElement message)
    {
        if (message.TryGetProperty("content", out var contentElement))
        {
            return contentElement.ValueKind == JsonValueKind.String ? contentElement.GetString() ?? string.Empty : string.Empty;
        }

        return string.Empty;
    }
}

// public static class ResponseFormatExtensions
// {
//     public static ResponseFormat ToResponseFormat(this ChatResponseFormat chatResponseFormat)
//     {
//         if (chatResponseFormat == null)
//         {
//             return new ResponseFormat { Type = ResponseFormatType.JsonObject };
//         }
//
//         var responseFormat = new ResponseFormat();
//
//         if (chatResponseFormat is ChatResponseFormatJson jsonFormat)
//         {
//             responseFormat.Type = ResponseFormatType.JsonObject;
//             if (jsonFormat.Schema is not null)
//             {
//                 try
//                 {
//                     responseFormat.Schema = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonFormat.Schema);
//                 }
//                 catch
//                 {
//                     responseFormat.Schema = new Dictionary<string, object>();
//                 }
//             }
//             else
//             {
//                 responseFormat.Schema = new Dictionary<string, object>();
//             }
//         }
//         else // ChatResponseFormatText
//         {
//             responseFormat.Type = ResponseFormatType.JsonSchema;
//         }
//
//         return responseFormat;
//     }
//
//     public static ChatResponseFormat ToChatResponseFormat(this ResponseFormat responseFormat)
//     {
//         if (responseFormat == null || responseFormat.Type == ResponseFormatType.JsonObject)
//         {
//             return ChatResponseFormat.Text;
//         }
//
//         var schema = responseFormat.Schema != null 
//             ? JsonSerializer.Serialize(responseFormat.Schema)
//             : "{}";
//
//         return ChatResponseFormat.ForJsonSchema(schema);
//     }
// }