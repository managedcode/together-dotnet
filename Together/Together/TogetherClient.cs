using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.AI;
using Together.Extensions;
using Together.Models.ChatCompletions;

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
        var messages = chatMessages.Select(s => new ChatCompletionMessage { Role = s.Role, Content = s.Text })
            .ToList();

        var request = new ChatCompletionRequest();

        if (options is not null)
        {
            request.Model = options.ModelId;
            request.Messages = messages;
            request.MaxTokens = options.MaxOutputTokens ?? 512;
            request.Stop = options.StopSequences?.ToList() ?? new List<string>();
            request.Temperature = options.Temperature;
            request.TopP = options.TopP;
            request.TopK = options.TopK;
            //RepetitionPenalty = options.RepetitionPenalty,
            request.PresencePenalty = options.PresencePenalty;
            request.FrequencyPenalty = options.FrequencyPenalty;
            //MinP = options.MinP,
            //LogitBias = options.LogitBias,
            request.Seed = Convert.ToInt32(options.Seed);
            request.Stream = false;
            //Logprobs = options.Logprobs,
            //Echo = options.Echo,
            //N = options.N,
            //SafetyModel = options.SafetyModel
        }

        var response = await httpClient.PostAsJsonAsync("completions", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        var completionResponse = await response.Content.ReadFromJsonAsync<ChatCompletionResponse>(cancellationToken);

        //  completionResponse.Choices.ForEach(c => new ChatMessage(c.));

        return new ChatCompletion(completionResponse.Choices
            .Select(c => new ChatMessage(c.Message.Role, c.Message.Content))
            .ToList());
    }

    public async IAsyncEnumerable<StreamingChatCompletionUpdate> CompleteStreamingAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        yield return new StreamingChatCompletionUpdate();
        throw new NotImplementedException();
        // var request = new ChatCompletionRequest
        // {
        //     Model = options.Model,
        //     Prompt = string.Join("\n", chatMessages),
        //     MaxTokens = options.MaxTokens ?? 512,
        //     Stop = options.Stop,
        //     Temperature = options.Temperature,
        //     TopP = options.TopP,
        //     TopK = options.TopK,
        //     RepetitionPenalty = options.RepetitionPenalty,
        //     PresencePenalty = options.PresencePenalty,
        //     FrequencyPenalty = options.FrequencyPenalty,
        //     MinP = options.MinP,
        //     LogitBias = options.LogitBias,
        //     Seed = options.Seed,
        //     Stream = true,
        //     Logprobs = options.Logprobs,
        //     Echo = options.Echo,
        //     N = options.N,
        //     SafetyModel = options.SafetyModel
        // };
        //
        // var response = await httpClient.PostAsJsonAsync("completions", request, cancellationToken);
        // response.EnsureSuccessStatusCode();
        // var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        //
        // using var reader = new StreamReader(stream);
        // while (!reader.EndOfStream)
        // {
        //     var line = await reader.ReadLineAsync();
        //     if (line != null)
        //     {
        //         var completionUpdate = JsonSerializer.Deserialize<StreamingChatCompletionUpdate>(line);
        //         if (completionUpdate != null)
        //         {
        //             yield return completionUpdate;
        //         }
        //     }
        // }
    }

    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        throw new NotImplementedException();
    }

    public ChatClientMetadata Metadata { get; }
}