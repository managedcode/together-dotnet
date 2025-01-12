using System.Net.Http.Headers;
using Microsoft.Extensions.AI;
using Together.Models.ChatCompletions;
using Together.Models.Completions;
using Together.Models.Embeddings;
using Together.Models.Images;
using Together.Models.Rerank;

namespace Together.Tests;

public class TogetherClientIntegraionTests
{
    private static readonly string API_KEY = "API_KEY";

    private TogetherClient CreateTogetherClient()
    {
        return new TogetherClient(API_KEY);
    }

    [Fact
#if !API_TEST
            (Skip = "This test is skipped because it requires a valid API key")
#endif
    ]
    public async Task CompletionTest()
    {
        var client = CreateTogetherClient();


        var responseAsync = await client.Completions.CreateAsync(new CompletionRequest
        {
            Prompt = "Hi",
            Model = "meta-llama/Meta-Llama-3-70B-Instruct-Turbo",
            MaxTokens = 20
        });

        Assert.NotEmpty(responseAsync.Choices.First()
            .Text);
    }

    [Fact
#if !API_TEST
            (Skip = "This test is skipped because it requires a valid API key")
#endif
    ]
    public async Task ChatCompletionTest()
    {
        var client = CreateTogetherClient();

        var responseAsync = await client.ChatCompletions.CreateAsync(new ChatCompletionRequest
        {
            Messages = new List<ChatCompletionMessage>
            {
                new()
                {
                    Role = ChatRole.User,
                    Content = "Hi"
                }
            },
            Model = "meta-llama/Meta-Llama-3-70B-Instruct-Turbo",
            MaxTokens = 20
        });

        Assert.NotEmpty(responseAsync.Choices.First()
            .Message.Content);
    }

    [Fact
#if !API_TEST
            (Skip = "This test is skipped because it requires a valid API key")
#endif
    ]
    public async Task StreamChatCompletionTest()
    {
        var client = CreateTogetherClient();

        var responseAsync = await client.ChatCompletions
            .CreateStreamAsync(new ChatCompletionRequest
            {
                Messages = new List<ChatCompletionMessage>
                {
                    new()
                    {
                        Role = ChatRole.User,
                        Content = "Hi"
                    }
                },
                Model = "meta-llama/Meta-Llama-3-70B-Instruct-Turbo",
                MaxTokens = 20,
                Stream = true
            })
            .Select(s => string.Join("", s.Choices.Select(c => c.Delta.Content)))
            .ToListAsync();


        Assert.NotEmpty(responseAsync);
    }

    [Fact
#if !API_TEST
            (Skip = "This test is skipped because it requires a valid API key")
#endif
    ]
    public async Task EmbeddingTest()
    {
        var client = CreateTogetherClient();

        var responseAsync = await client.Embeddings.CreateAsync(new EmbeddingRequest
        {
            Input = "Hi",
            Model = "togethercomputer/m2-bert-80M-2k-retrieval"
        });

        Assert.NotNull(responseAsync.Data);
    }

    [Fact
#if !API_TEST
            (Skip = "This test is skipped because it requires a valid API key")
#endif
    ]
    public async Task ImageTest()
    {
        var client = CreateTogetherClient();

        var responseAsync = await client.Images.GenerateAsync(new ImageRequest
        {
            Model = "black-forest-labs/FLUX.1-dev",
            Prompt = "Cats eating popcorn",
            N = 1,
            Steps = 10,
            Height = 512,
            Width = 512
        });

        Assert.NotEmpty(responseAsync.Data.First()
            .Url);
    }

    [Fact
#if !API_TEST
            (Skip = "This test is skipped because it requires a valid API key")
#endif
    ]
    public async Task ModelsTest()
    {
        var client = CreateTogetherClient();

        var responseAsync = await client.Models.ListModelsAsync();

        Assert.NotEmpty(responseAsync);
    }

    [Fact
#if !API_TEST
            (Skip = "This test is skipped because it requires a valid API key")
#endif
    ]
    public async Task RerankTest()
    {
        var client = CreateTogetherClient();

        var responseAsync = await client.Rerank.CreateAsync(new RerankRequest());

        Assert.NotEmpty(responseAsync.Results);
    }

    [Fact
#if !API_TEST
            (Skip = "This test is skipped because it requires a valid API key")
#endif
    ]
    public async Task WrongModelTest()
    {
        var client = CreateTogetherClient();

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            var responseAsync = await client.Images.GenerateAsync(new ImageRequest
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