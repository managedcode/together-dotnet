using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.SemanticKernel;

namespace Together.SemanticKernel;

/// <summary>
///     Together AI Execution Settings.
/// </summary>
[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public sealed class TogetherPromptExecutionSettings : PromptExecutionSettings
{
    private bool? _echo;
    private Dictionary<string, float>? _logitBias;
    private int? _logprobs;
    private float? _minP;
    private float? _repetitionPenalty;
    private string? _safetyModel;
    private ulong? _seed;
    private List<string>? _stop;

    private int? _topK;

    public static JsonSerializerOptions ReadPermissive { get; } = new()
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip
    };

    [JsonPropertyName("top_k")]
    public int? TopK
    {
        get => _topK;
        set
        {
            ThrowIfFrozen();
            _topK = value;
        }
    }

    [JsonPropertyName("min_p")]
    public float? MinP
    {
        get => _minP;
        set
        {
            ThrowIfFrozen();
            _minP = value;
        }
    }

    [JsonPropertyName("logit_bias")]
    public Dictionary<string, float>? LogitBias
    {
        get => _logitBias;
        set
        {
            ThrowIfFrozen();
            _logitBias = value;
        }
    }

    [JsonPropertyName("repetition_penalty")]
    public float? RepetitionPenalty
    {
        get => _repetitionPenalty;
        set
        {
            ThrowIfFrozen();
            _repetitionPenalty = value;
        }
    }

    [JsonPropertyName("stop")]
    public List<string>? Stop
    {
        get => _stop;
        set
        {
            ThrowIfFrozen();
            _stop = value;
        }
    }

    [JsonPropertyName("logprobs")]
    public int? Logprobs
    {
        get => _logprobs;
        set
        {
            ThrowIfFrozen();
            _logprobs = value;
        }
    }

    [JsonPropertyName("echo")]
    public bool? Echo
    {
        get => _echo;
        set
        {
            ThrowIfFrozen();
            _echo = value;
        }
    }

    [JsonPropertyName("safety_model")]
    public string? SafetyModel
    {
        get => _safetyModel;
        set
        {
            ThrowIfFrozen();
            _safetyModel = value;
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

    /// <summary>
    ///     Gets the specialization for the Together execution settings.
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

    public override PromptExecutionSettings Clone()
    {
        return new TogetherPromptExecutionSettings
        {
            ModelId = ModelId,
            TopK = TopK,
            MinP = MinP,
            LogitBias = LogitBias != null ? new Dictionary<string, float>(LogitBias) : null,
            RepetitionPenalty = RepetitionPenalty,
            Stop = Stop != null ? new List<string>(Stop) : null,
            Logprobs = Logprobs,
            Echo = Echo,
            SafetyModel = SafetyModel,
            Seed = Seed,
            ExtensionData = ExtensionData != null ? new Dictionary<string, object>(ExtensionData) : null
        };
    }
}