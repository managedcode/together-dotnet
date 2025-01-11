using System.Runtime.CompilerServices;
using System.Text.Json;
using Together.Models.ChatCompletions;

namespace Together.Clients;

public class ChatCompletionClient(HttpClient httpClient) : BaseClient(httpClient)
{
    public async Task<ChatCompletionResponse> CreateAsync(ChatCompletionRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<ChatCompletionRequest, ChatCompletionResponse>("/chat/completions", request, cancellationToken);
    }

    public async IAsyncEnumerable<ChatCompletionChunk> CreateStreamAsync(ChatCompletionRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var responseMessage = await SendRequestAsync<ChatCompletionRequest, HttpResponseMessage>("/chat/completions", request, cancellationToken);

        await using var stream = await responseMessage.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        while (await reader.ReadLineAsync(cancellationToken) is string line)
        {
            if (!line.StartsWith("data:"))
            {
                continue;
            }

            var eventData = line.Substring("data:".Length)
                .Trim();
            if (eventData is null or "[DONE]")
            {
                break;
            }

            var result = JsonSerializer.Deserialize<ChatCompletionChunk>(eventData);

            if (result is not null)
            {
                yield return result;
            }
        }
    }
}