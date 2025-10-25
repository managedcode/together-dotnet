using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Together.Models.Evaluations;

public class EvaluationJob
{
    [JsonPropertyName("workflow_id")]
    public string WorkflowId { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}

public class EvaluationListResponse
{
    [JsonPropertyName("data")]
    public List<EvaluationJob> Data { get; set; } = new();
}

public class EvaluationListOptions
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("limit")]
    public int? Limit { get; set; }
}

public class EvaluationCreateResponse
{
    [JsonPropertyName("workflow_id")]
    public string WorkflowId { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
}

public class EvaluationStatusResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("results")]
    public JsonElement? Results { get; set; }
}

public class EvaluationRetrieveResponse
{
    [JsonPropertyName("workflow_id")]
    public string WorkflowId { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("results")]
    public JsonElement? Results { get; set; }

    [JsonPropertyName("parameters")]
    public Dictionary<string, JsonElement>? Parameters { get; set; }
}

public class EvaluationAllowedModelsResponse
{
    [JsonPropertyName("models")]
    public List<string> Models { get; set; } = new();
}

public class JudgeModelConfig
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("model_source")]
    public string ModelSource { get; set; } = string.Empty;

    [JsonPropertyName("system_template")]
    public string SystemTemplate { get; set; } = string.Empty;

    [JsonPropertyName("external_api_token")]
    public string? ExternalApiToken { get; set; }

    [JsonPropertyName("external_base_url")]
    public string? ExternalBaseUrl { get; set; }
}

public class ModelRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("model_source")]
    public string ModelSource { get; set; } = string.Empty;

    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; }

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    [JsonPropertyName("system_template")]
    public string SystemTemplate { get; set; } = string.Empty;

    [JsonPropertyName("input_template")]
    public string InputTemplate { get; set; } = string.Empty;

    [JsonPropertyName("external_api_token")]
    public string? ExternalApiToken { get; set; }

    [JsonPropertyName("external_base_url")]
    public string? ExternalBaseUrl { get; set; }
}

public class EvaluationCreateRequest
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("judge")]
    public JudgeModelConfig Judge { get; set; } = new();

    [JsonPropertyName("input_data_file_path")]
    public string InputDataFilePath { get; set; } = string.Empty;

    [JsonPropertyName("labels")]
    public List<string>? Labels { get; set; }

    [JsonPropertyName("pass_labels")]
    public List<string>? PassLabels { get; set; }

    [JsonPropertyName("min_score")]
    public double? MinScore { get; set; }

    [JsonPropertyName("max_score")]
    public double? MaxScore { get; set; }

    [JsonPropertyName("pass_threshold")]
    public double? PassThreshold { get; set; }

    [JsonPropertyName("model_to_evaluate")]
    public object? ModelToEvaluate { get; set; }

    [JsonPropertyName("model_a")]
    public object? ModelA { get; set; }

    [JsonPropertyName("model_b")]
    public object? ModelB { get; set; }
}
