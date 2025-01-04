using System.Runtime.CompilerServices;
using Microsoft.Extensions.AI;

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