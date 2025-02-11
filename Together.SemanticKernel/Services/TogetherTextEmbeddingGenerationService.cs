using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;
using Together.Models.Embeddings;

namespace Together.SemanticKernel.Services;

[Experimental("SKEXP0001")]
public class TogetherTextEmbeddingGenerationService : ITextEmbeddingGenerationService
{
    private const int BatchSize = 20; // Adjust based on API limits
    private static readonly Meter s_meter = new("Microsoft.SemanticKernel.Connectors.Together");

    private static readonly Counter<int> s_embeddingRequestsCounter =
        s_meter.CreateCounter<int>("semantic_kernel.connectors.together.embedding.requests");

    private static readonly Counter<int> s_embeddingTokensCounter =
        s_meter.CreateCounter<int>("semantic_kernel.connectors.together.embedding.tokens");

    private readonly Dictionary<string, object?> _attributes = new();

    private readonly TogetherClient _client;
    private readonly ILogger _logger;
    private readonly string _model;

    public TogetherTextEmbeddingGenerationService(TogetherClient togetherClient, string model, ILogger? logger = null)
    {
        ArgumentNullException.ThrowIfNull(togetherClient);
        ArgumentException.ThrowIfNullOrWhiteSpace(model);

        _client = togetherClient;
        _model = model;
        _logger = logger ?? NullLogger.Instance;
        _attributes.Add("ModelId", model);
    }

    public IReadOnlyDictionary<string, object?> Attributes => _attributes;

    public async Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data, Kernel? kernel = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(data);

        try
        {
            using var activity = Activity.Current?.Source.StartActivity("GenerateEmbeddings");
            var results = new List<ReadOnlyMemory<float>>();

            // Process in batches to avoid potential API limits
            for (var i = 0; i < data.Count; i += BatchSize)
            {
                var batchItems = data.Skip(i)
                    .Take(BatchSize)
                    .ToList();
                var response = await _client.Embeddings.CreateAsync(new EmbeddingRequest
                {
                    Input = batchItems,
                    Model = _model
                }, cancellationToken);

                if (response?.Data == null)
                {
                    throw new KernelException("No embedding data received from Together.AI");
                }

                s_embeddingRequestsCounter.Add(1);
                // s_embeddingTokensCounter.Add(response.Usage?.TotalTokens ?? 0);
                //
                // _logger.LogInformation(
                //     "Generated {EmbeddingCount} embeddings using {TokenCount} tokens",
                //     response.Data.Count,
                //     response.Usage?.TotalTokens);

                results.AddRange(response.Data.Select(d => new ReadOnlyMemory<float>(d.Embedding.ToArray())));
            }

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating embeddings");
            throw new KernelException("Failed to generate embeddings", ex);
        }
    }
}