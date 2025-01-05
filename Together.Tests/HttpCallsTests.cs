using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using Microsoft.Extensions.AI;
using Together.Models.ChatCompletions;
using Together.Models.Completions;
using Together.Models.Embeddings;
using Together.Models.Images;
using Together.Models.Rerank;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

namespace Together.Tests;

public class HttpCallsTests
{
    static string API_KEY= "API_KEY";
    
    private HttpClient CreateHttpClient()
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(TogetherConstants.TIMEOUT_SECS);
        httpClient.BaseAddress = new Uri(TogetherConstants.BASE_URL);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", API_KEY);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return httpClient;
    }
    
    [Fact]
    public async Task CompletionTest()
    {
        var client = new TogetherClient(CreateHttpClient());
        

        var responseAsync = await client.Completions.CreateAsync(new CompletionRequest()
        {
            Prompt = "Hi",
            Model = "meta-llama/Meta-Llama-3-70B-Instruct-Turbo",
            MaxTokens = 20
        });
        
        Assert.NotEmpty(responseAsync.Choices.First().Text);
    }
    
    [Fact]
    public async Task ChatCompletionTest()
    {
        var client = new TogetherClient(CreateHttpClient());

        var responseAsync = await client.ChatCompletions.CreateAsync(new ChatCompletionRequest
        {
            Messages = new List<ChatCompletionMessage>()
            {
                new ChatCompletionMessage()
                {
                    Role = ChatRole.User,
                    Content = "Hi"
                }
            },
            Model = "meta-llama/Meta-Llama-3-70B-Instruct-Turbo",
            MaxTokens = 20
        });
        
        Assert.NotEmpty(responseAsync.Choices.First().Message.Content);
    }
    
    [Fact]
    public async Task StreamChatCompletionTest()
    {
        var client = new TogetherClient(CreateHttpClient());

        var responseAsync = await client.ChatCompletions.CreateStreamAsync(new ChatCompletionRequest
        {
            Messages = new List<ChatCompletionMessage>()
            {
                new ChatCompletionMessage()
                {
                    Role = ChatRole.User,
                    Content = "Hi"
                }
            },
            Model = "meta-llama/Meta-Llama-3-70B-Instruct-Turbo",
            MaxTokens = 20,
            Stream = true
        }).Select(s=> string.Join("",s.Choices.Select(c=>c.Delta.Content))).ToListAsync();
        
        
        
        Assert.NotEmpty(responseAsync);
    }
    
    [Fact]
    public async Task EmbeddingTest()
    {
        var client = new TogetherClient(CreateHttpClient());

        var responseAsync = await client.Embeddings.CreateAsync(new EmbeddingRequest()
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

        var responseAsync = await client.Images.GenerateAsync(new ImageRequest()
        {
            Model = "black-forest-labs/FLUX.1-dev",
            Prompt = "Cats eating popcorn",
            N = 1,
            Steps = 10,
            Height = 512,
            Width = 512
        });
        
        Assert.NotEmpty(responseAsync.Data.First().Url);
    }
    
    [Fact]
    public async Task ModelsTest()
    {
        var client = new TogetherClient(CreateHttpClient());

        var responseAsync = await client.Models.ListModelsAsync();
        
        Assert.NotEmpty(responseAsync);
    }
    
    [Fact]
    public async Task RerankTest()
    {
        var client = new TogetherClient(CreateHttpClient());

        var responseAsync = await client.Rerank.CreateAsync(new RerankRequest()
        {
            
        });
        
        Assert.NotEmpty(responseAsync.Results);
    }
    
    [Fact]
    public async Task WrongModelTest()
    {
        var client = new TogetherClient(CreateHttpClient());

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            var responseAsync = await client.Images.GenerateAsync(new ImageRequest()
            {
                Model = "Wring-Model",
                Prompt = "so wrong",
                N = 1,
                Steps = 10,
                Height = 512,
                Width = 512
            });
        });
    }
}