using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.AI;
using Together.Models.ChatCompletions;
using Together.Models.Completions;
using Together.Models.Embeddings;
using Together.Models.Images;

namespace Together;

public class TogetherClient(HttpClient httpClient) : IChatClient
{
    public void Dispose()
    {
        httpClient.Dispose();
    }

    public async Task<ChatCompletion> CompleteAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async IAsyncEnumerable<StreamingChatCompletionUpdate> CompleteStreamingAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        yield return new StreamingChatCompletionUpdate();
        throw new NotImplementedException();
    }

    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        throw new NotImplementedException();
    }

    public ChatClientMetadata Metadata { get; }
}

public class TogetherChatCompletionsClient(HttpClient httpClient)
{

    public JsonSerializerOptions GetJsonSerializerOptions => new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
    };
    
    public async Task<CompletionResponse> GetCompletionResponseAsync(CompletionRequest request, CancellationToken cancellationToken = default)
    {
        var responseMessage = await httpClient.PostAsJsonAsync(requestUri: "/completions", value: request, options: GetJsonSerializerOptions, cancellationToken);
        
        var content = await responseMessage.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
        responseMessage.EnsureSuccessStatusCode();
        
        
        return await responseMessage.Content.ReadFromJsonAsync<CompletionResponse>(cancellationToken: cancellationToken);
    }
    
    public async Task<ChatCompletionResponse> GetChatCompletionResponseAsync(ChatCompletionRequest request, CancellationToken cancellationToken = default)
    {
        
        var responseMessage = await httpClient.PostAsJsonAsync(requestUri: "/chat/completions", value: request,  options: GetJsonSerializerOptions, cancellationToken);
      
        var content = await responseMessage.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
        responseMessage.EnsureSuccessStatusCode();
        
        return await responseMessage.Content.ReadFromJsonAsync<ChatCompletionResponse>(cancellationToken: cancellationToken);
    }
    
    public async Task<EmbeddingResponse> GetEmbeddingResponseAsync(EmbeddingRequest request, CancellationToken cancellationToken = default)
    {
        var responseMessage = await httpClient.PostAsJsonAsync(requestUri: "/embeddings", value: request,  options: GetJsonSerializerOptions, cancellationToken);
      
        var content = await responseMessage.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
        responseMessage.EnsureSuccessStatusCode();
        
        return await responseMessage.Content.ReadFromJsonAsync<EmbeddingResponse>(cancellationToken: cancellationToken);
    }
    
    public async Task<ImageResponse> GetImageResponseAsync(ImageRequest request, CancellationToken cancellationToken = default)
    {
        var responseMessage = await httpClient.PostAsJsonAsync(requestUri: "/images/generations", value: request,  options: GetJsonSerializerOptions, cancellationToken);
      
        var content = await responseMessage.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
        responseMessage.EnsureSuccessStatusCode();
        
        return await responseMessage.Content.ReadFromJsonAsync<ImageResponse>(cancellationToken: cancellationToken);
    }

        
}