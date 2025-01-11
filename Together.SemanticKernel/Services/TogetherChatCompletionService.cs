using System.Runtime.CompilerServices;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.TextGeneration;
using Together;
using Together.Models.ChatCompletions;
using TextContent = Microsoft.SemanticKernel.TextContent;

namespace Together.SemanticKernel.Services;

public sealed class TogetherChatCompletionService : IChatCompletionService, ITextGenerationService
{
    private readonly TogetherClient _client;
    private readonly string _model;
    private readonly Dictionary<string, object?> _attributes = new();

    public TogetherChatCompletionService(TogetherClient togetherClient, string model)
    {
        ArgumentNullException.ThrowIfNull(togetherClient);
        ArgumentException.ThrowIfNullOrWhiteSpace(model);

        _client = togetherClient;
        _model = model;
        _attributes.Add("ModelId", model);
    }

    public IReadOnlyDictionary<string, object?> Attributes => _attributes;

    public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new ChatCompletionRequest
            {
                Model = _model,
                Messages = chatHistory.ToChatCompletionMessages()
                    .ToList(),
                Stream = false
            };
            var response = await _client.ChatCompletions.CreateAsync(request, cancellationToken);

            var choice = response.Choices.FirstOrDefault()?.Message;
            return choice == null 
                ? Array.Empty<ChatMessageContent>() 
                : new[] { new ChatMessageContent(AuthorRole.Assistant, choice.Content) };
        }
        catch (Exception ex)
        {
            throw new KernelException("Error getting chat completion", ex);
        }
    }

    public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {

        var request = new ChatCompletionRequest
        {
            Model = _model,
            Messages = chatHistory.ToChatCompletionMessages()
                .ToList(),
            Stream = true
        };
        
        var stream = _client.ChatCompletions.CreateStreamAsync(request, cancellationToken);

        await foreach (var chunk in stream)
        {
            foreach (var choice in chunk.Choices)
            {
                yield return new StreamingChatMessageContent(AuthorRole.Assistant, choice.Delta?.Content, null, choice.Index.GetValueOrDefault(), chunk.Model);
            }
        }
    }

    public Task<IReadOnlyList<TextContent>> GetTextContentsAsync(
        string prompt,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default)
    {
        // Convert text prompt to chat format
        var chatHistory = new ChatHistory();
        chatHistory.AddUserMessage(prompt);
        return GetChatMessageContentsAsync(chatHistory, executionSettings, kernel, cancellationToken)
            .ContinueWith(t => (IReadOnlyList<TextContent>)t.Result
                .Select(m => new TextContent(m.Content))
                .ToList(), cancellationToken);
    }

    public async IAsyncEnumerable<StreamingTextContent> GetStreamingTextContentsAsync(
        string prompt,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var chatHistory = new ChatHistory();
        chatHistory.AddUserMessage(prompt);

        await foreach (var message in GetStreamingChatMessageContentsAsync(chatHistory, executionSettings, kernel, cancellationToken))
        {
            yield return new StreamingTextContent(message.Content);
        }
    }
}