using System.Text.Json.Serialization;
using Microsoft.SemanticKernel;

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

    public override PromptExecutionSettings Clone()
    {
        return new TogetherTextToImageExecutionSettings
        {
            ModelId = ModelId,
            Steps = Steps,
            Seed = Seed,
            N = N,
            Height = Height,
            Width = Width,
            NegativePrompt = NegativePrompt,
            ExtensionData = ExtensionData != null ? new Dictionary<string, object>(ExtensionData) : null
        };
    }
}