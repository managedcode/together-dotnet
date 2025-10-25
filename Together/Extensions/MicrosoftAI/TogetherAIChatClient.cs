using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Together.Clients;
using Together.Models.ChatCompletions;
using Together.Models.Common;

namespace Together.Extensions.MicrosoftAI;

public class TogetherAIChatClient : IChatClient
{
    private readonly ChatCompletionClient _chatClient;
    private readonly string _defaultModel;

    public TogetherAIChatClient(ChatCompletionClient chatClient, string defaultModel)
    {
        _chatClient = chatClient ?? throw new ArgumentNullException(nameof(chatClient));
        _defaultModel = defaultModel;
    }

    public async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options, CancellationToken cancellationToken = default)
    {
        var request = messages.ToChatCompletionRequest(options, _defaultModel);
        var response = await _chatClient.CreateAsync(request, cancellationToken);

        var choice = response.Choices.FirstOrDefault();
        var message = choice?.Message;
        var chatMessage = new ChatMessage(message?.Role ?? ChatRole.Assistant, message?.Content ?? string.Empty)
        {
            AuthorName = message?.Role.ToString()
        };

        var chatResponse = new ChatResponse(new List<ChatMessage> { chatMessage })
        {
            ResponseId = response.Id,
            ModelId = response.Model,
            CreatedAt = response.Created.HasValue ? DateTimeOffset.FromUnixTimeSeconds(response.Created.Value) : null,
            FinishReason = choice?.FinishReason is FinishReason reason ? MapFinishReason(reason) : null,
            Usage = response.Usage is null ? null : new UsageDetails
            {
                InputTokenCount = response.Usage.PromptTokens,
                OutputTokenCount = response.Usage.CompletionTokens,
                TotalTokenCount = response.Usage.TotalTokens
            }
        };

        return chatResponse;
    }

    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = messages.ToChatCompletionRequest(options, _defaultModel);
        request.Stream = true;

        await foreach (var chunk in _chatClient.CreateStreamAsync(request, cancellationToken))
        {
            var delta = chunk.Choices.FirstOrDefault()?.Delta;
            if (delta is null)
            {
                continue;
            }

            if (string.IsNullOrEmpty(delta.Content))
            {
                continue;
            }

            var update = new ChatResponseUpdate(ChatRole.Assistant, delta.Content);

            update.ResponseId = chunk.Id;
            update.ModelId = chunk.Model;
            update.CreatedAt = chunk.Created.HasValue ? DateTimeOffset.FromUnixTimeSeconds(chunk.Created.Value) : null;

            yield return update;
        }
    }

    public object? GetService(Type serviceType, object? serviceKey)
    {
        if (serviceType == typeof(ChatCompletionClient))
        {
            return _chatClient;
        }

        return null;
    }

    public void Dispose()
    {
    }

    private static ChatFinishReason? MapFinishReason(FinishReason reason) => reason.Value.ToLowerInvariant() switch
    {
        "length" => ChatFinishReason.Length,
        "stop" => ChatFinishReason.Stop,
        "tool_calls" => ChatFinishReason.ToolCalls,
        "content_filter" => ChatFinishReason.ContentFilter,
        _ => null
    };
}
