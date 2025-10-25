using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Together.Clients;
using Together.Models.Embeddings;

namespace Together.Extensions.MicrosoftAI;

public class TogetherAIEmbeddingGenerator : IEmbeddingGenerator<string, Embedding<float>>
{
    private readonly EmbeddingClient _embeddingClient;
    private readonly string _defaultModel;

    public TogetherAIEmbeddingGenerator(EmbeddingClient embeddingClient, string defaultModel)
    {
        _embeddingClient = embeddingClient ?? throw new ArgumentNullException(nameof(embeddingClient));
        _defaultModel = defaultModel;
    }

    public async Task<GeneratedEmbeddings<Embedding<float>>> GenerateAsync(IEnumerable<string> inputs, EmbeddingGenerationOptions? options, CancellationToken cancellationToken = default)
    {
        var inputList = inputs?.ToList() ?? throw new ArgumentNullException(nameof(inputs));
        if (inputList.Count == 0)
        {
            throw new ArgumentException("At least one input is required for embedding generation.", nameof(inputs));
        }

        var model = options?.ModelId ?? _defaultModel;
        if (string.IsNullOrWhiteSpace(model))
        {
            throw new InvalidOperationException("A model identifier must be provided for embedding generation.");
        }

        var request = new EmbeddingRequest
        {
            Model = model,
            Input = inputList.Count == 1 ? inputList[0] : inputList
        };

        var response = await _embeddingClient.CreateAsync(request, cancellationToken);
        var embeddings = response.Data
            .OrderBy(d => d.Index)
            .Select(d => new Embedding<float>(d.Embedding.ToArray()))
            .ToList();

        return new GeneratedEmbeddings<Embedding<float>>(embeddings);
    }

    public object? GetService(Type serviceType, object? serviceKey)
    {
        if (serviceType == typeof(EmbeddingClient))
        {
            return _embeddingClient;
        }

        return null;
    }

    public void Dispose()
    {
    }
}
