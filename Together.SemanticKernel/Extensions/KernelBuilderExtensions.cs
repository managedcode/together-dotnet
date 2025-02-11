using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.TextGeneration;
using Microsoft.SemanticKernel.TextToImage;
using Together.SemanticKernel.Services;

namespace Together.SemanticKernel.Extensions;

public static class KernelBuilderExtensions
{
    public static IKernelBuilder AddTogetherChatCompletion(this IKernelBuilder builder, string model, string apiKey, string? endpoint = null,
        HttpClient? httpClient = null, string? serviceId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model);

        if (string.IsNullOrEmpty(serviceId))
        {
            builder.Services.AddSingleton<IChatCompletionService>(serviceProvider =>
                new TogetherChatCompletionService(new TogetherClient(apiKey, GetHttpClient(httpClient, serviceProvider), endpoint), model,
                    serviceProvider.GetService<ILogger<TogetherChatCompletionService>>()));

            builder.Services.AddSingleton<ITextGenerationService>(serviceProvider =>
                new TogetherChatCompletionService(new TogetherClient(apiKey, GetHttpClient(httpClient, serviceProvider), endpoint), model,
                    serviceProvider.GetService<ILogger<TogetherChatCompletionService>>()));
        }
        else
        {
            builder.Services.AddKeyedSingleton<IChatCompletionService>(serviceId,
                (serviceProvider, _) =>
                    new TogetherChatCompletionService(new TogetherClient(apiKey, GetHttpClient(httpClient, serviceProvider), endpoint), model,
                        serviceProvider.GetService<ILogger<TogetherChatCompletionService>>()));

            builder.Services.AddKeyedSingleton<ITextGenerationService>(serviceId,
                (serviceProvider, _) =>
                    new TogetherChatCompletionService(new TogetherClient(apiKey, GetHttpClient(httpClient, serviceProvider), endpoint), model,
                        serviceProvider.GetService<ILogger<TogetherChatCompletionService>>()));
        }


        return builder;
    }

    [Experimental("SKEXP0001")]
    public static IKernelBuilder AddTogetherTextEmbeddingGeneration(this IKernelBuilder builder, string model, string apiKey, string? endpoint = null,
        HttpClient? httpClient = null, string? serviceId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model);

        if (string.IsNullOrEmpty(serviceId))
        {
            builder.Services.AddSingleton<ITextEmbeddingGenerationService>(serviceProvider =>
                new TogetherTextEmbeddingGenerationService(new TogetherClient(apiKey, GetHttpClient(httpClient, serviceProvider), endpoint), model));
        }
        else
        {
            builder.Services.AddKeyedSingleton<ITextEmbeddingGenerationService>(serviceId,
                (serviceProvider, _) =>
                    new TogetherTextEmbeddingGenerationService(new TogetherClient(apiKey, GetHttpClient(httpClient, serviceProvider), endpoint),
                        model));
        }


        return builder;
    }

    [Experimental("SKEXP0001")]
    public static IKernelBuilder AddTogetherTextToImage(this IKernelBuilder builder, string model, string apiKey, string? endpoint = null,
        HttpClient? httpClient = null, string? serviceId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model);

        if (string.IsNullOrEmpty(serviceId))
        {
            builder.Services.AddSingleton<ITextToImageService>(serviceProvider =>
                new TogetherTextToImageService(new TogetherClient(apiKey, GetHttpClient(httpClient, serviceProvider), endpoint), model));
        }
        else
        {
            builder.Services.AddKeyedSingleton<ITextToImageService>(serviceId,
                (serviceProvider, _) =>
                    new TogetherTextToImageService(new TogetherClient(apiKey, GetHttpClient(httpClient, serviceProvider), endpoint), model));
        }

        return builder;
    }

    private static HttpClient GetHttpClient(HttpClient? httpClient, IServiceProvider serviceProvider)
    {
        return httpClient ?? serviceProvider.GetService<HttpClient>() ?? new HttpClient();
    }
}