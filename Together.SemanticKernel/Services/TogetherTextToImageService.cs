using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.TextToImage;
using Together.Models.ChatCompletions;
using Together.Models.Images;

namespace Together.SemanticKernel.Services;

[Experimental("SKEXP0001")]
public class TogetherTextToImageService : ITextToImageService
{
    private readonly TogetherClient _client;
    private readonly string _model;
    private readonly Dictionary<string, object?> _attributes = new();
    private readonly ILogger _logger;

    public TogetherTextToImageService(TogetherClient togetherClient, string model, ILogger? logger = null)
    {
        ArgumentNullException.ThrowIfNull(togetherClient);
        ArgumentException.ThrowIfNullOrWhiteSpace(model);

        _client = togetherClient;
        _model = model;
        _logger = logger ?? NullLogger.Instance;
        _attributes.Add("ModelId", model);
    }

    public IReadOnlyDictionary<string, object?> Attributes => _attributes;

    public async Task<IReadOnlyList<ImageContent>> GetImageContentsAsync(
        TextContent input,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentException.ThrowIfNullOrWhiteSpace(input.Text);

        try
        {
            using var activity = Activity.Current?.Source.StartActivity("GenerateImage");

            var request = new ImageRequest
            {
                Model = _model,
                Prompt = input.Text,
                Width = 1024,  // default values
                Height = 1024,
            };

            if (executionSettings is TogetherTextToImageExecutionSettings settings)
            {
                ApplyTogetherSettings(request, settings);
            }
            else if (executionSettings?.ExtensionData != null)
            {
                ApplyExecutionSettings(request, executionSettings.ExtensionData);
            }

            ValidateImageRequest(request);

            var response = await _client.Images.GenerateAsync(request, cancellationToken);
            
            if (response?.Data == null || response.Data.Count == 0)
            {
                throw new KernelException("No image data received from Together.AI");
            }

            var results = new List<ImageContent>();
            foreach (var image in response.Data)
            {
                if (!IsValidImageOutput(image))
                {
                    _logger.LogWarning("Image generation produced no usable output");
                    continue;
                }

                results.Add(CreateImageContent(image, input));
            }

            if (results.Count == 0)
            {
                throw new KernelException("No valid images were generated");
            }

            _logger.LogInformation(
                "Generated {Count} images for prompt: {Prompt}",
                results.Count,
                input.Text);

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate images for prompt: {Prompt}", input.Text);
            throw new KernelException("Image generation failed", ex);
        }
    }

    private static void ApplyTogetherSettings(ImageRequest request, TogetherTextToImageExecutionSettings settings)
    {
        if (settings.Width.HasValue)
        {
            request.Width = settings.Width.Value;
        }
        if (settings.Height.HasValue)
        {
            request.Height = settings.Height.Value;
        }
        if (!string.IsNullOrEmpty(settings.NegativePrompt))
        {
            request.NegativePrompt = settings.NegativePrompt;
        }
        if (settings.Seed.HasValue)
        {
            request.Seed = settings.Seed.Value;
        }
    }

    private static void ApplyExecutionSettings(ImageRequest request, IDictionary<string, object> settings)
    {
        if (settings.TryGetValue("width", out var width) && width is int widthValue)
        {
            request.Width = widthValue;
        }
        if (settings.TryGetValue("height", out var height) && height is int heightValue)
        {
            request.Height = heightValue;
        }
        if (settings.TryGetValue("negative_prompt", out var negativePrompt) && negativePrompt is string negativePromptValue)
        {
            request.NegativePrompt = negativePromptValue;
        }
        if (settings.TryGetValue("seed", out var seed) && seed is ulong seedValue)
        {
            request.Seed = seedValue;
        }
    }

    private static void ValidateImageRequest(ImageRequest request)
    {
        if (request.Width is < 128 or > 1024)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Width), "Width must be between 128 and 1024 pixels");
        }
        if (request.Height is < 128 or > 1024)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Height), "Height must be between 128 and 1024 pixels");
        }
    }

    private static bool IsValidImageOutput(ImageChoicesData image)
        => !string.IsNullOrEmpty(image.Url) || image.B64Json != null;

    private static ImageContent CreateImageContent(ImageChoicesData image, TextContent input)
    {
        var metadata = new Dictionary<string, object?>
        {
            { "model", input.ModelId },
            { "prompt", input.Text }
        };

        return !string.IsNullOrEmpty(image.Url)
            ? new ImageContent(new Uri(image.Url))
            : new ImageContent(image.B64Json);
    }
}