using System.Text.Json.Serialization;
using Microsoft.SemanticKernel;
using Together.Models.Images;

namespace Together.SemanticKernel;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public sealed class TogetherTextToImageExecutionSettings : PromptExecutionSettings
{
    private int? _height = 1024;
    private int? _n = 1;
    private string? _negativePrompt;
    private ulong? _seed;
    private int? _steps = 20;
    private int? _width = 1024;
    private string? _prompt;
    private string? _imageUrl;
    private List<ImageLora>? _imageLoras;
    private string _responseFormat = "url";

    [JsonPropertyName("steps")]
    public int? Steps
    {
        get => _steps;
        set
        {
            ThrowIfFrozen();
            _steps = value;
        }
    }

    [JsonPropertyName("seed")]
    public ulong? Seed
    {
        get => _seed;
        set
        {
            ThrowIfFrozen();
            _seed = value;
        }
    }

    [JsonPropertyName("n")]
    public int? N
    {
        get => _n;
        set
        {
            ThrowIfFrozen();
            _n = value;
        }
    }

    [JsonPropertyName("height")]
    public int? Height
    {
        get => _height;
        set
        {
            ThrowIfFrozen();
            _height = value;
        }
    }

    [JsonPropertyName("width")]
    public int? Width
    {
        get => _width;
        set
        {
            ThrowIfFrozen();
            _width = value;
        }
    }
    
    
    [JsonPropertyName("prompt")]
    public string? Prompt
    {
        get => _prompt;
        set
        {
            ThrowIfFrozen();
            _prompt = value;
        }
    }

    [JsonPropertyName("negative_prompt")]
    public string? NegativePrompt
    {
        get => _negativePrompt;
        set
        {
            ThrowIfFrozen();
            _negativePrompt = value;
        }
    }

    
    [JsonPropertyName("image_url")]
    public string? ImageUrl
    {
        get => _imageUrl;
        set
        {
            ThrowIfFrozen();
            _imageUrl = value;
        }
    }

    [JsonPropertyName("image_loras")]
    public List<ImageLora>? ImageLoras
    {
        get => _imageLoras;
        set
        {
            ThrowIfFrozen();
            _imageLoras = value;
        }
    }

    [JsonPropertyName("response_format")]
    public string ResponseFormat
    {
        get => _responseFormat;
        set
        {
            ThrowIfFrozen();
            _responseFormat = value;
        }
    }

    public override PromptExecutionSettings Clone()
    {
        return new TogetherTextToImageExecutionSettings
        {
            ModelId = ModelId,
            Prompt = Prompt,
            Steps = Steps,
            Seed = Seed,
            N = N,
            Height = Height,
            Width = Width,
            NegativePrompt = NegativePrompt,
            ImageUrl = ImageUrl,
            ImageLoras = ImageLoras?.ToList(),
            ResponseFormat = ResponseFormat,
            ExtensionData = ExtensionData != null ? new Dictionary<string, object>(ExtensionData) : null
        };
    }
}