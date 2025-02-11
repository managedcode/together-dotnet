using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.SemanticKernel;

namespace Together.SemanticKernel;

/// <summary>
/// Together AI Execution Settings.
/// </summary>
[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public sealed class TogetherPromptExecutionSettings : PromptExecutionSettings
{
    public static JsonSerializerOptions ReadPermissive { get; } = new()
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
    };
    
    /// <summary>
    /// Gets the specialization for the Together execution settings.
    /// </summary>
    public static TogetherPromptExecutionSettings FromExecutionSettings(PromptExecutionSettings? executionSettings)
    {
        switch (executionSettings)
        {
            case null:
                return new TogetherPromptExecutionSettings();
            case TogetherPromptExecutionSettings settings:
                return settings;
        }

        var json = JsonSerializer.Serialize(executionSettings);
        var togetherSettings = JsonSerializer.Deserialize<TogetherPromptExecutionSettings>(json, ReadPermissive);

        return togetherSettings!;
    }

    [JsonPropertyName("top_k")]
    public int? TopK
    {
        get => this._topK;
        set
        {
            ThrowIfFrozen();
            this._topK = value;
        }
    }

    [JsonPropertyName("min_p")]
    public float? MinP
    {
        get => this._minP;
        set
        {
            ThrowIfFrozen();
            this._minP = value;
        }
    }

    [JsonPropertyName("logit_bias")]
    public Dictionary<string, float>? LogitBias
    {
        get => this._logitBias;
        set
        {
            ThrowIfFrozen();
            this._logitBias = value;
        }
    }

    [JsonPropertyName("repetition_penalty")]
    public float? RepetitionPenalty
    {
        get => this._repetitionPenalty;
        set
        {
            ThrowIfFrozen();
            this._repetitionPenalty = value;
        }
    }

    [JsonPropertyName("stop")]
    public List<string>? Stop
    {
        get => this._stop;
        set
        {
            ThrowIfFrozen();
            this._stop = value;
        }
    }

    [JsonPropertyName("logprobs")]
    public int? Logprobs
    {
        get => this._logprobs;
        set
        {
            ThrowIfFrozen();
            this._logprobs = value;
        }
    }

    [JsonPropertyName("echo")]
    public bool? Echo
    {
        get => this._echo;
        set
        {
            ThrowIfFrozen();
            this._echo = value;
        }
    }

    [JsonPropertyName("safety_model")]
    public string? SafetyModel
    {
        get => this._safetyModel;
        set
        {
            ThrowIfFrozen();
            this._safetyModel = value;
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

    public override PromptExecutionSettings Clone()
    {
        return new TogetherPromptExecutionSettings
        {
            ModelId = this.ModelId,
            TopK = this.TopK,
            MinP = this.MinP,
            LogitBias = this.LogitBias != null ? new Dictionary<string, float>(this.LogitBias) : null,
            RepetitionPenalty = this.RepetitionPenalty,
            Stop = this.Stop != null ? new List<string>(this.Stop) : null,
            Logprobs = this.Logprobs,
            Echo = this.Echo,
            SafetyModel = this.SafetyModel,
            Seed = this.Seed,
            ExtensionData = this.ExtensionData != null ? new Dictionary<string, object>(this.ExtensionData) : null
        };
    }

    private int? _topK;
    private float? _minP;
    private Dictionary<string, float>? _logitBias;
    private float? _repetitionPenalty;
    private List<string>? _stop;
    private int? _logprobs;
    private bool? _echo;
    private string? _safetyModel;
    private ulong? _seed;
}
