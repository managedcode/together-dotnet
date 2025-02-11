using System.Text.Json.Serialization;
using Microsoft.SemanticKernel;

namespace Together.SemanticKernel;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public sealed class TogetherTextToImageExecutionSettings : PromptExecutionSettings
{
    [JsonPropertyName("steps")]
    public int? Steps
    {
        get => this._steps;
        set
        {
            ThrowIfFrozen();
            this._steps = value;
        }
    }

    [JsonPropertyName("seed")]
    public ulong? Seed
    {
        get => this._seed;
        set
        {
            ThrowIfFrozen();
            this._seed = value;
        }
    }

    [JsonPropertyName("n")]
    public int? N
    {
        get => this._n;
        set
        {
            ThrowIfFrozen();
            this._n = value;
        }
    }

    [JsonPropertyName("height")]
    public int? Height
    {
        get => this._height;
        set
        {
            ThrowIfFrozen();
            this._height = value;
        }
    }

    [JsonPropertyName("width")]
    public int? Width
    {
        get => this._width;
        set
        {
            ThrowIfFrozen();
            this._width = value;
        }
    }

    [JsonPropertyName("negative_prompt")]
    public string? NegativePrompt
    {
        get => this._negativePrompt;
        set
        {
            ThrowIfFrozen();
            this._negativePrompt = value;
        }
    }

    public override PromptExecutionSettings Clone()
    {
        return new TogetherTextToImageExecutionSettings
        {
            ModelId = this.ModelId,
            Steps = this.Steps,
            Seed = this.Seed,
            N = this.N,
            Height = this.Height,
            Width = this.Width,
            NegativePrompt = this.NegativePrompt,
            ExtensionData = this.ExtensionData != null ? new Dictionary<string, object>(this.ExtensionData) : null
        };
    }

    private int? _steps = 20;
    private ulong? _seed;
    private int? _n = 1;
    private int? _height = 1024;
    private int? _width = 1024;
    private string? _negativePrompt;
}
