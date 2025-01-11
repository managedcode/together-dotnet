namespace Together.Extensions;

public static class ChatOptionsConverter
{
    // public static ChatCompletionRequest ToChatCompletionRequest(ChatOptions options, IList<ChatMessage> chatMessages)
    // {
    //     ArgumentException.ThrowIfNullOrWhiteSpace(options.ModelId);
    //
    //     return new ChatCompletionRequest
    //     {
    //         Model = options.ModelId,
    //         Messages = chatMessages.Select(s => new ChatCompletionMessage { Role = s.Role, Content = s.Text })
    //             .ToList(),
    //         MaxTokens = options.MaxOutputTokens,
    //         Stop = options.StopSequences?.ToList(),
    //         Temperature = options.Temperature,
    //         TopP = options.TopP,
    //         TopK = options.TopK,
    //         PresencePenalty = options.PresencePenalty,
    //         FrequencyPenalty = options.FrequencyPenalty,
    //         //Seed = options.Seed,
    //         ResponseFormat = options.ResponseFormat,
    //         //Tools = options.Tools.Select(t => t.ToString()).ToList(),
    //         Stream = false
    //     };
    // }
}