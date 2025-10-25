using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Together.Clients;
using Together.Models.Images;

namespace Together.Extensions.MicrosoftAI;

public class TogetherAIImageClient : IChatClient
{
    private readonly ImageClient _imageClient;
    private readonly string _defaultModel;

    public TogetherAIImageClient(ImageClient imageClient, string defaultModel)
    {
        _imageClient = imageClient ?? throw new ArgumentNullException(nameof(imageClient));
        _defaultModel = defaultModel;
    }

    public async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options, CancellationToken cancellationToken = default)
    {
        var promptMessage = messages.LastOrDefault(m => m.Role == ChatRole.User) ?? throw new InvalidOperationException("Image generation requires a user prompt.");
        var prompt = promptMessage.Text ?? string.Empty;

        var request = BuildImageRequest(prompt, options);
        var response = await _imageClient.GenerateAsync(request, cancellationToken);

        var contents = response.Data
            .OrderBy(d => d.Index)
            .SelectMany(CreateContent)
            .ToList();

        var assistantMessage = new ChatMessage(ChatRole.Assistant, contents);

        return new ChatResponse(new List<ChatMessage> { assistantMessage })
        {
            ResponseId = response.Id,
            ModelId = response.Model
        };
    }

    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var response = await GetResponseAsync(messages, options, cancellationToken);
        var contents = response.Messages.First().Contents;
        var update = new ChatResponseUpdate(ChatRole.Assistant, contents)
        {
            ModelId = response.ModelId,
            ResponseId = response.ResponseId,
            FinishReason = ChatFinishReason.Stop
        };

        yield return update;
    }

    public object? GetService(Type serviceType, object? serviceKey)
    {
        if (serviceType == typeof(ImageClient))
        {
            return _imageClient;
        }

        return null;
    }

    public void Dispose()
    {
    }

    private ImageRequest BuildImageRequest(string prompt, ChatOptions? options)
    {
        var model = options?.ModelId ?? _defaultModel;
        if (string.IsNullOrWhiteSpace(model))
        {
            throw new InvalidOperationException("A model identifier must be provided for image generation.");
        }

        var request = new ImageRequest
        {
            Model = model,
            Prompt = prompt,
            Height = options?.AdditionalProperties?.TryGetValue("height", out var height) == true && int.TryParse(height?.ToString(), out var h) ? h : (int?)null,
            Width = options?.AdditionalProperties?.TryGetValue("width", out var width) == true && int.TryParse(width?.ToString(), out var w) ? w : (int?)null,
            Steps = options?.AdditionalProperties?.TryGetValue("steps", out var steps) == true && int.TryParse(steps?.ToString(), out var s) ? s : (int?)null,
            ResponseFormat = options?.AdditionalProperties?.TryGetValue("response_format", out var format) == true ? format?.ToString() ?? "url" : "url",
            NegativePrompt = options?.AdditionalProperties?.TryGetValue("negative_prompt", out var negative) == true ? negative?.ToString() : null
        };

        return request;
    }

    private static IEnumerable<AIContent> CreateContent(ImageChoicesData data)
    {
        if (!string.IsNullOrEmpty(data.Url) && Uri.TryCreate(data.Url, UriKind.Absolute, out var uri))
        {
            yield return new UriContent(uri, "image/png");
        }

        if (!string.IsNullOrEmpty(data.B64Json))
        {
            var bytes = Convert.FromBase64String(data.B64Json);
            yield return new DataContent(bytes, "image/png");
        }
    }
}
