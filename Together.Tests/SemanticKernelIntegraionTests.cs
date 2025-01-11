using System.Net.Http.Headers;
using Microsoft.Extensions.AI;
using Together.Models.ChatCompletions;
using Together.Models.Completions;
using Together.Models.Embeddings;
using Together.Models.Images;
using Together.Models.Rerank;

namespace Together.Tests;

public class SemanticKernelIntegraionTests
{
    private static readonly string API_KEY = "API_KEY";

    private TogetherClient CreateTogetherClient()
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(TogetherConstants.TIMEOUT_SECS);
        return new TogetherClient(httpClient, API_KEY);
    }

//     [Fact
// #if !API_TEST
//             (Skip = "This test is skipped because it requires a valid API key")
// #endif
//     ]
    
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
