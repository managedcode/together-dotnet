using System.Diagnostics.CodeAnalysis;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.TextToImage;

namespace Together.SemanticKernel.Services;

[Experimental("SKEXP0001")]
public class TogetherTextToImageService : ITextToImageService
{
    public IReadOnlyDictionary<string, object?> Attributes { get; }

    public async Task<IReadOnlyList<ImageContent>> GetImageContentsAsync(TextContent input, PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null, CancellationToken cancellationToken = new())
    {
        throw new NotImplementedException();
    }
}