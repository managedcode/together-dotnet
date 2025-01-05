using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Together.Models.ChatCompletions;
using Together.Models.Completions;
using Together.Models.Embeddings;
using Together.Models.Error;
using Together.Models.Images;

namespace Together;

public class TogetherClient(HttpClient httpClient)
{
    private async Task<TResponse> SendRequestAsync<TRequest, TResponse>(string requestUri, TRequest request, CancellationToken cancellationToken)
    {
        var responseMessage = await httpClient.PostAsJsonAsync(requestUri, request, cancellationToken);

        if (responseMessage.IsSuccessStatusCode)
        {
            var result = await responseMessage.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
            return result!;
        }

        var errorResponse = await responseMessage.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: cancellationToken);
        if (errorResponse?.Error != null)
        {
            throw new Exception(errorResponse.Error.Message);
        }

        var statusCode = responseMessage.StatusCode;
        var errorContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
        throw new Exception($"Request failed with status code {statusCode}: {errorContent}");
    }

    public async Task<CompletionResponse> GetCompletionResponseAsync(CompletionRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<CompletionRequest, CompletionResponse>("/completions", request, cancellationToken);
    }

    public async Task<ChatCompletionResponse> GetChatCompletionResponseAsync(ChatCompletionRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<ChatCompletionRequest, ChatCompletionResponse>("/chat/completions", request, cancellationToken);
    }
    
    public async IAsyncEnumerable<ChatCompletionChunk> GetStreamChatCompletionResponseAsync(ChatCompletionRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var responseMessage = await httpClient.PostAsJsonAsync("/chat/completions", request, cancellationToken);

        if (!responseMessage.IsSuccessStatusCode)
        {
            var errorResponse = await responseMessage.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: cancellationToken);
            if (errorResponse?.Error != null)
            {
                throw new Exception(errorResponse.Error.Message);
            }

            var statusCode = responseMessage.StatusCode;
            var errorContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
            throw new Exception($"Request failed with status code {statusCode}: {errorContent}");
        }

        await using var stream = await responseMessage.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        while (await reader.ReadLineAsync(cancellationToken) is string line)
        {
            if (!line.StartsWith("data:"))
                continue;

            var eventData = line.Substring("data:".Length).Trim();
            if (eventData is null or "[DONE]")
                break;

            var result = JsonSerializer.Deserialize<ChatCompletionChunk>(eventData);

            if (result is not null)
                yield return result;
        }
    }

    public async Task<EmbeddingResponse> GetEmbeddingResponseAsync(EmbeddingRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<EmbeddingRequest, EmbeddingResponse>("/embeddings", request, cancellationToken);
    }

    public async Task<ImageResponse> GetImageResponseAsync(ImageRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<ImageRequest, ImageResponse>("/images/generations", request, cancellationToken);
    }
}