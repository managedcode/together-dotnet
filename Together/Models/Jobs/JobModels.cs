using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Together.Models.Jobs;

public class JobStatusUpdate
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = string.Empty;
}

public class JobArgs
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("modelName")]
    public string? ModelName { get; set; }

    [JsonPropertyName("modelSource")]
    public string? ModelSource { get; set; }
}

public class JobRetrieveResponse
{
    [JsonPropertyName("args")]
    public JobArgs Args { get; set; } = new();

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; } = string.Empty;

    [JsonPropertyName("job_id")]
    public string JobId { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("status_updates")]
    public List<JobStatusUpdate> StatusUpdates { get; set; } = new();

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("updated_at")]
    public string UpdatedAt { get; set; } = string.Empty;
}

public class JobListItem
{
    [JsonPropertyName("args")]
    public JobArgs Args { get; set; } = new();

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; } = string.Empty;

    [JsonPropertyName("job_id")]
    public string JobId { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("status_updates")]
    public List<JobStatusUpdate> StatusUpdates { get; set; } = new();

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("updated_at")]
    public string UpdatedAt { get; set; } = string.Empty;
}

public class JobListResponse
{
    [JsonPropertyName("data")]
    public List<JobListItem> Data { get; set; } = new();
}
