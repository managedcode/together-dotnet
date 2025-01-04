using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using Microsoft.Extensions.AI;
using Together.Models.ChatCompletions;
using Together.Models.Completions;
using Together.Models.Embeddings;
using Together.Models.Images;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

namespace Together.Tests;

public class HttpCallsTests
{
    static string API_KEY= "PUT YOUR KEY HERE";
    
    private HttpClient CreateHttpClient()
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(TogetherConstants.TIMEOUT_SECS);
        httpClient.BaseAddress = new Uri(TogetherConstants.BASE_URL);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", API_KEY);
        return httpClient;
    }
    
    [Fact]
    public async Task CompletionTest()
    {
        var client = new TogetherClient(CreateHttpClient());
        

        var responseAsync = await client.GetCompletionResponseAsync(new CompletionRequest()
        {
            Prompt = "Hi",
            Model = "meta-llama/Llama-3.3-70B-Instruct-Turbo",
            MaxTokens = 20
        });
        
        Assert.NotNull(responseAsync.Usage);
    }
    
    [Fact]
    public async Task ChatCompletionTest()
    {
        var client = new TogetherClient(CreateHttpClient());

        var responseAsync = await client.GetChatCompletionResponseAsync(new ChatCompletionRequest
        {
            Messages = new List<ChatCompletionMessage>()
            {
                new ChatCompletionMessage()
                {
                    Role = ChatRole.User,
                    Content = "Hi"
                }
            },
            Model = "meta-llama/Llama-3.3-70B-Instruct-Turbo",
            MaxTokens = 20
        });
        
        Assert.NotNull(responseAsync.Usage);
    }
    
    [Fact]
    public async Task StreamChatCompletionTest()
    {
        var client = new TogetherClient(CreateHttpClient());

        var responseAsync = await client.GetStreamChatCompletionResponseAsync(new ChatCompletionRequest
        {
            Messages = new List<ChatCompletionMessage>()
            {
                new ChatCompletionMessage()
                {
                    Role = ChatRole.User,
                    Content = "Hi"
                }
            },
            Model = "meta-llama/Llama-3.3-70B-Instruct-Turbo",
            MaxTokens = 20,
            Stream = true
        }).Select(s=> string.Join("",s.Choices.Select(c=>c.Delta.Content))).ToListAsync();
        
        
        
        Assert.NotNull(responseAsync);
    }
    
    [Fact]
    public async Task EmbeddingTest()
    {
        var client = new TogetherClient(CreateHttpClient());

        var responseAsync = await client.GetEmbeddingResponseAsync(new EmbeddingRequest()
        {
            Input = "Hi",
            Model = "togethercomputer/m2-bert-80M-2k-retrieval",
        });
        
        Assert.NotNull(responseAsync.Data);
    }
    
    [Fact]
    public async Task ImageTest()
    {
        var client = new TogetherClient(CreateHttpClient());

        var responseAsync = await client.GetImageResponseAsync(new ImageRequest()
        {
            Model = "black-forest-labs/FLUX.1-dev",
            Prompt = "Cats eating popcorn",
            N = 1,
            Steps = 10,
            Height = 512,
            Width = 512
        });
        
        Assert.NotNull(responseAsync.Data);
    }
}