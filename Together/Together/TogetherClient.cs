using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.AI;
using Together.Models.ChatCompletions;
using Together.Models.Completions;
using Together.Models.Embeddings;
using Together.Models.Error;
using Together.Models.Images;

namespace Together;

public class TogetherClient(HttpClient httpClient) : IChatClient
{
    public void Dispose()
    {
        httpClient.Dispose();
    }

    async Task<ChatCompletion> IChatClient.CompleteAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    async IAsyncEnumerable<StreamingChatCompletionUpdate> IChatClient.CompleteStreamingAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        yield return new StreamingChatCompletionUpdate();
        throw new NotImplementedException();
    }

    object? IChatClient.GetService(Type serviceType, object? serviceKey = null)
    {
        throw new NotImplementedException();
    }

    ChatClientMetadata IChatClient.Metadata { get; }
    
      private JsonSerializerOptions GetJsonSerializerOptions => new(JsonSerializerDefaults.Web)
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
    
    public async IAsyncEnumerable<ChatCompletionChunk> GetStreamChatCompletionResponseAsync(ChatCompletionRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var responseMessage = await httpClient.PostAsJsonAsync(requestUri: "/chat/completions", value: request,  options: GetJsonSerializerOptions, cancellationToken);
        responseMessage.EnsureSuccessStatusCode();

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
        var responseMessage = await httpClient.PostAsJsonAsync(requestUri: "/embeddings", value: request,  options: GetJsonSerializerOptions, cancellationToken);
      
        var content = await responseMessage.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
        responseMessage.EnsureSuccessStatusCode();
        
        return await responseMessage.Content.ReadFromJsonAsync<EmbeddingResponse>(cancellationToken: cancellationToken);
    }
    
    public async Task<ImageResponse> GetImageResponseAsync(ImageRequest request, CancellationToken cancellationToken = default)
    {
        var responseMessage = await httpClient.PostAsJsonAsync(requestUri: "/images/generations", value: request,  options: GetJsonSerializerOptions, cancellationToken);
      
        var content = await responseMessage.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
        
        if(responseMessage.IsSuccessStatusCode)
            return await responseMessage.Content.ReadFromJsonAsync<ImageResponse>(cancellationToken: cancellationToken);
 
        
        var responce = await responseMessage.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: cancellationToken);
        throw new Exception(responce.Error.Message);
    }
}
