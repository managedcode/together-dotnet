using System.Net.Http.Headers;
using FluentAssertions;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Together.Models.ChatCompletions;
using Together.Models.Completions;
using Together.Models.Embeddings;
using Together.Models.Images;
using Together.Models.Rerank;
using Together.SemanticKernel;

namespace Together.Tests;

public class SemanticKernelIntegraionTests
{
    private static readonly string API_KEY = "API_KEY";
    

//     [Fact
// #if !API_TEST
//             (Skip = "This test is skipped because it requires a valid API key")
// #endif
//     ]
    
    [Fact]
    public async Task InvokePromptAsyncTest()
    {
        var kernel = Kernel.CreateBuilder()
            .AddTogetherChatCompletion("meta-llama/Meta-Llama-3-70B-Instruct-Turbo", API_KEY).Build();

        var answer = await kernel.InvokePromptAsync("Hi");
        answer.RenderedPrompt.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task CompletionTest()
    {
        var kernel = Kernel.CreateBuilder()
            .AddTogetherChatCompletion("meta-llama/Meta-Llama-3-70B-Instruct-Turbo", API_KEY).Build();

        bool call = false;
        kernel.ImportPluginFromFunctions("currentTime", "return current time",
        [
            kernel.CreateFunctionFromMethod(() =>
            {
                call = true;
                return new System.DateTime().ToString();
            }, "GetCurrentTime"),
        ]);
        
        var answer = await kernel.InvokePromptAsync("What is the current time?");
        answer.RenderedPrompt.Should().NotBeNullOrEmpty();
        call.Should()
            .BeTrue();
    }
}