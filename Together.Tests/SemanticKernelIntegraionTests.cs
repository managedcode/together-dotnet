using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.TextToImage;
using Shouldly;
using Together.Models.ChatCompletions;
using Together.Models.Completions;
using Together.Models.Embeddings;
using Together.Models.Images;
using Together.Models.Rerank;
using Together.SemanticKernel;
using Together.SemanticKernel.Extensions;
using TextContent = Microsoft.SemanticKernel.TextContent;

namespace Together.Tests;

public class SemanticKernelIntegraionTests
{
    private static readonly string API_KEY = "API_KEY";
    



    [Fact
#if !API_TEST
            (Skip = "This test is skipped because it requires a valid API key")
#endif
    ]
    [Experimental("SKEXP0010")]
    public async Task InvokeOpenAiPromptAsyncTest()
    {
        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion("meta-llama/Meta-Llama-3-70B-Instruct-Turbo", new Uri("https://api.together.xyz/v1"),  API_KEY)
            .Build();

        var answer = await kernel.InvokePromptAsync("Hi");
        answer.RenderedPrompt.ShouldNotBeEmpty();
    }
    
    [Fact
#if !API_TEST
            (Skip = "This test is skipped because it requires a valid API key")
#endif
    ]
    [Experimental("SKEXP0010")]
    public async Task FunctionCallTest()
    {
        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion("mistralai/Mistral-7B-Instruct-v0.1", new Uri("https://api.together.xyz/v1"),  API_KEY)
            .Build();
        
        bool call = false;
        
        kernel.Plugins.AddFromFunctions("time_plugin",
        [
            KernelFunctionFactory.CreateFromMethod(
                method: () =>
                {
                    call = true;
                    return DateTime.Now;
                },
                functionName: "get_time",
                description: "Get the current time"
            )
        ]);

        var message = await kernel.GetRequiredService<IChatCompletionService>()
            .GetChatMessageContentAsync("What is the current time?", new OpenAIPromptExecutionSettings()
            {
               FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            }, kernel);
        
        var answer = await kernel.InvokePromptAsync("What is the current time?");
        answer.RenderedPrompt.ShouldNotBeEmpty();
        call.ShouldBeTrue();
    }
    
    [Fact
#if !API_TEST
            (Skip = "This test is skipped because it requires a valid API key")
#endif
    ]
    public async Task InvokePromptAsyncTest()
    {
        var kernel = Kernel.CreateBuilder()
            .AddTogetherChatCompletion("mistralai/Mistral-7B-Instruct-v0.1", API_KEY).Build();

        var answer = await kernel.InvokePromptAsync("Hi");
        answer.RenderedPrompt.ShouldNotBeEmpty();
    }
    
    [Fact
#if !API_TEST
            (Skip = "This test is skipped because it requires a valid API key")
#endif
    ]
    public async Task CompletionTest()
    {
        var kernel = Kernel.CreateBuilder()
            .AddTogetherChatCompletion("mistralai/Mistral-7B-Instruct-v0.1", API_KEY).Build();

        bool call = false;
        
        kernel.Plugins.AddFromFunctions("time_plugin",
        [
            KernelFunctionFactory.CreateFromMethod(
                method: () =>
                {
                    call = true;
                    return DateTime.Now;
                },
                functionName: "get_time",
                description: "Get the current time"
            )
        ]);
        
        var message = await kernel.GetRequiredService<IChatCompletionService>()
            .GetChatMessageContentAsync("What is the current time?", new OpenAIPromptExecutionSettings()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            }, kernel);
        
        var answer = await kernel.InvokePromptAsync("What is the current time?");
        answer.RenderedPrompt.ShouldNotBeEmpty();
        call.ShouldBeTrue();
    }
    
    [Fact
#if !API_TEST
            (Skip = "This test is skipped because it requires a valid API key")
#endif
    ]
    [Experimental("SKEXP0001")]
    public async Task ImageTest()
    {
        var kernel = Kernel.CreateBuilder()
            .AddTogetherTextToImage("black-forest-labs/FLUX.1-dev", API_KEY).Build();


        var imageService = kernel.GetRequiredService<ITextToImageService>();
        
        var images = await imageService.GetImageContentsAsync(new TextContent()
        {
            Text = "Cats eating popcorn"
        }, new TogetherTextToImageExecutionSettings()
        {
            Height = 512,
            Width = 512
        });
        
        
        images.Count.ShouldBePositive();
        images.First().Uri.ShouldNotBeNull();
    }
    
    [Fact
#if !API_TEST
            (Skip = "This test is skipped because it requires a valid API key")
#endif
    ]
    [Experimental("SKEXP0001")]
    public async Task Embedded()
    {
        var kernel = Kernel.CreateBuilder()
            .AddTogetherTextEmbeddingGeneration("togethercomputer/m2-bert-80M-2k-retrieval", API_KEY).Build();


        var embedding = kernel.GetRequiredService<ITextEmbeddingGenerationService>();

        var embeddings = await embedding.GenerateEmbeddingsAsync(new List<string>()
        {
            "Cats eating popcorn"
        });

        
        
        embeddings.Count.ShouldBePositive();
    }
}

