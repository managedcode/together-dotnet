using System.Diagnostics.CodeAnalysis;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;

namespace Together.SemanticKernel.Services;

[Experimental("SKEXP0001")]
public class TogetherTextEmbeddingGenerationService : ITextEmbeddingGenerationService
{
    public async Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data, Kernel? kernel = null,
        CancellationToken cancellationToken = new())
    {
        throw new NotImplementedException();
    }

    public IReadOnlyDictionary<string, object?> Attributes { get; }
}