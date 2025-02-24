using System.Diagnostics.CodeAnalysis;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.TextToImage;
using Shouldly;
using Together.SemanticKernel;
using Together.SemanticKernel.Extensions;
using TextContent = Microsoft.SemanticKernel.TextContent;

namespace Together.Tests;

public class SemanticKernelIntegraionTests
{
    private static readonly string API_KEY = "API_KEY";


    private string TextModel = "mistralai/Mistral-7B-Instruct-v0.1";//"meta-llama/Llama-3.3-70B-Instruct-Turbo";
    private string ImageModel = "black-forest-labs/FLUX.1-depth";
    private string EmbeddedModel = "togethercomputer/m2-bert-80M-2k-retrieval";

    [Fact
#if !API_TEST
            (Skip = "This test is skipped because it requires a valid API key")
#endif
    ]
    [Experimental("SKEXP0010")]
    public async Task InvokeOpenAiPromptAsyncTest()
    {
        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(TextModel, new Uri("https://api.together.xyz/v1"), API_KEY)
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
    public async Task OpenAIFunctionCallTest()
    {
        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(TextModel, new Uri("https://api.together.xyz/v1"), API_KEY)
            .Build();

        var call = false;

        kernel.Plugins.AddFromFunctions("time_plugin", [
            KernelFunctionFactory.CreateFromMethod(() =>
            {
                call = true;
                return DateTime.Now;
            }, "get_time", "Get the current time")
        ]);

        var message = await kernel.GetRequiredService<IChatCompletionService>()
            .GetChatMessageContentAsync("What is the current time?", new OpenAIPromptExecutionSettings
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
    [Experimental("SKEXP0010")]
    public async Task FunctionCallTest()
    {
        var kernel = Kernel.CreateBuilder()
            .AddTogetherChatCompletion(TextModel, API_KEY)
            .Build();

        var call = false;

        kernel.Plugins.AddFromFunctions("time_plugin", [
            KernelFunctionFactory.CreateFromMethod(() =>
            {
                call = true;
                return DateTime.Now;
            }, "get_time", "Get the current time")
        ]);

        var message = await kernel.GetRequiredService<IChatCompletionService>()
            .GetChatMessageContentAsync("What is the current time?", new TogetherPromptExecutionSettings
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
            .AddTogetherChatCompletion(TextModel, API_KEY)
            .Build();

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
            .AddTogetherChatCompletion(TextModel, API_KEY)
            .Build();

        var call1 = false;
        var call2 = false;

        kernel.Plugins.AddFromFunctions("time_plugin", [
            KernelFunctionFactory.CreateFromMethod(() =>
            {
                call1 = true;
                return DateTime.Now;
            }, "get_time", "Get the current time")
        ]);
        kernel.Plugins.AddFromFunctions("news_plugin", [
            KernelFunctionFactory.CreateFromMethod((string day) =>
            {
                call2 = true;
                return $"Today is {day}, and we have 5 news items.";
            }, "get_news_for_day", "get news for specific day")
        ]);

        var message1 = await kernel.GetRequiredService<IChatCompletionService>()
            .GetChatMessageContentAsync("What is the current time?", new TogetherPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            }, kernel);
        
        var message2 = await kernel.GetRequiredService<IChatCompletionService>()
            .GetChatMessageContentAsync("how many news we have for monday?", new TogetherPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            }, kernel);

        var answer1 = await kernel.InvokePromptAsync("What is the current time?");
        var answer2 = await kernel.InvokePromptAsync("how many news we have for monday?");
        message1.Content.ShouldNotBeEmpty();
        message2.Content.ShouldNotBeEmpty();
        answer1.Metadata.Count.ShouldBePositive();
        answer2.Metadata.Count.ShouldBePositive();
        call1.ShouldBeTrue();
        call2.ShouldBeTrue();
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
            .AddTogetherTextToImage(ImageModel, API_KEY)
            .Build();


        var imageService = kernel.GetRequiredService<ITextToImageService>();

        var images = await imageService.GetImageContentsAsync(new TextContent
        {
            Text = "Cats eating popcorn"
        }, new TogetherTextToImageExecutionSettings
        {
            Height = 512,
            Width = 512
        });


        images.Count.ShouldBePositive();
        images.First()
            .Uri
            .ShouldNotBeNull();
    }
    
    [Fact
#if !API_TEST
            (Skip = "This test is skipped because it requires a valid API key")
#endif
    ]
    [Experimental("SKEXP0001")]
    public async Task ImageTestLora()
    {
        var kernel = Kernel.CreateBuilder()
            .AddTogetherTextToImage(ImageModel, API_KEY)
            .Build();


        var imageService = kernel.GetRequiredService<ITextToImageService>();

        var images = await imageService.GetImageContentsAsync(new TextContent
        {
            Text = "Make a cat base on photo eating popcorn"
        }, new TogetherTextToImageExecutionSettings
        {
            Height = 512,
            Width = 512,
            ImageUrl = "https://avatars.githubusercontent.com/u/1024025?v=4"
        });


        images.Count.ShouldBePositive();
        images.First()
            .Uri
            .ShouldNotBeNull();
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
            .AddTogetherTextEmbeddingGeneration(EmbeddedModel, API_KEY)
            .Build();


        var embedding = kernel.GetRequiredService<ITextEmbeddingGenerationService>();

        var embeddings = await embedding.GenerateEmbeddingsAsync(new List<string>
        {
            "Cats eating popcorn"
        });


        embeddings.Count.ShouldBePositive();
    }
}