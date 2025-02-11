using Microsoft.Extensions.AI;
using Together.Models.ChatCompletions;

namespace Together.SemanticKernel;

public static class ChatOptionsConverter
{
    public static ChatCompletionRequest ToChatCompletionRequest(ChatOptions options, IList<ChatMessage> chatMessages)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(options.ModelId);

        return new ChatCompletionRequest
        {
            Model = options.ModelId,
            Messages = chatMessages.Select(s => new ChatCompletionMessage { Role = s.Role, Content = s.Text })
                .ToList(),
            MaxTokens = options.MaxOutputTokens,
            Stop = options.StopSequences?.ToList(),
            Temperature = options.Temperature,
            TopP = options.TopP,
            TopK = options.TopK,
            PresencePenalty = options.PresencePenalty,
            FrequencyPenalty = options.FrequencyPenalty,
            Seed = Convert.ToUInt64(options.Seed),
            ResponseFormat = ConvertResponseFormat(options.ResponseFormat),
            Stream = false
        };
    }

    private static ResponseFormat ConvertResponseFormat(ChatResponseFormat chatResponseFormat)
    {
        return chatResponseFormat switch
        {
            ChatResponseFormatText => new ResponseFormat
            {
                Type = ResponseFormatType.JsonObject,
                Schema = new Dictionary<string, object>()
            },
            ChatResponseFormatJson jsonFormat => new ResponseFormat
            {
                Type = ResponseFormatType.JsonSchema,
                Schema = new Dictionary<string, object>
                {
                    { "schema", jsonFormat.Schema },
                    { "schemaName", jsonFormat.SchemaName },
                    { "schemaDescription", jsonFormat.SchemaDescription }
                }
            },
            _ => throw new ArgumentException("Unknown ChatResponseFormat type")
        };
    }
}