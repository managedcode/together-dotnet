using System;
using System.Text.Json.Serialization;

namespace Together.Models.Batch;

public enum BatchJobStatus
{
    Validating,
    InProgress,
    Completed,
    Failed,
    Expired,
    Cancelled,
    Canceling
}

public enum BatchEndpoint
{
    [JsonPropertyName("/v1/completions")]
    Completions,

    [JsonPropertyName("/v1/chat/completions")]
    ChatCompletions
}

public class BatchJob
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("user_id")]
    public string UserId { get; set; } = string.Empty;

    [JsonPropertyName("input_file_id")]
    public string InputFileId { get; set; } = string.Empty;

    [JsonPropertyName("file_size_bytes")]
    public long FileSizeBytes { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("job_deadline")]
    public DateTime JobDeadline { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("endpoint")]
    public string Endpoint { get; set; } = string.Empty;

    [JsonPropertyName("progress")]
    public double Progress { get; set; }

    [JsonPropertyName("model_id")]
    public string? ModelId { get; set; }

    [JsonPropertyName("output_file_id")]
    public string? OutputFileId { get; set; }

    [JsonPropertyName("error_file_id")]
    public string? ErrorFileId { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("completed_at")]
    public DateTime? CompletedAt { get; set; }
}

public class BatchCreateRequest
{
    [JsonPropertyName("input_file_id")]
    public string InputFileId { get; set; } = string.Empty;

    [JsonPropertyName("endpoint")]
    public string Endpoint { get; set; } = string.Empty;

    [JsonPropertyName("completion_window")]
    public string CompletionWindow { get; set; } = "24h";
}

public class BatchCreateResponse
{
    [JsonPropertyName("job")]
    public BatchJob? Job { get; set; }
}
