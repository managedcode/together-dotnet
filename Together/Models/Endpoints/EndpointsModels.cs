using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Together.Models.Endpoints;

public class Autoscaling
{
    [JsonPropertyName("min_replicas")]
    public int MinReplicas { get; set; }

    [JsonPropertyName("max_replicas")]
    public int MaxReplicas { get; set; }
}

public class EndpointPricing
{
    [JsonPropertyName("cents_per_minute")]
    public double CentsPerMinute { get; set; }
}

public class HardwareSpec
{
    [JsonPropertyName("gpu_type")]
    public string GpuType { get; set; } = string.Empty;

    [JsonPropertyName("gpu_link")]
    public string GpuLink { get; set; } = string.Empty;

    [JsonPropertyName("gpu_memory")]
    public double GpuMemory { get; set; }

    [JsonPropertyName("gpu_count")]
    public int GpuCount { get; set; }
}

public class HardwareAvailability
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
}

public class HardwareWithStatus
{
    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("pricing")]
    public EndpointPricing Pricing { get; set; } = new();

    [JsonPropertyName("specs")]
    public HardwareSpec Specs { get; set; } = new();

    [JsonPropertyName("availability")]
    public HardwareAvailability? Availability { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
}

public class HardwareListResponse
{
    [JsonPropertyName("data")]
    public List<HardwareWithStatus> Data { get; set; } = new();

    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;
}

public abstract class BaseEndpoint
{
    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("owner")]
    public string Owner { get; set; } = string.Empty;

    [JsonPropertyName("state")]
    public string State { get; set; } = string.Empty;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class ListEndpoint : BaseEndpoint
{
}

public class DedicatedEndpoint : BaseEndpoint
{
    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    [JsonPropertyName("hardware")]
    public string Hardware { get; set; } = string.Empty;

    [JsonPropertyName("autoscaling")]
    public Autoscaling Autoscaling { get; set; } = new();
}

public class EndpointCreateRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("hardware")]
    public string Hardware { get; set; } = string.Empty;

    [JsonPropertyName("autoscaling")]
    public Autoscaling Autoscaling { get; set; } = new();

    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("disable_prompt_cache")]
    public bool DisablePromptCache { get; set; } = true;

    [JsonPropertyName("disable_speculative_decoding")]
    public bool DisableSpeculativeDecoding { get; set; } = true;

    [JsonPropertyName("state")]
    public string State { get; set; } = "STARTED";

    [JsonPropertyName("inactive_timeout")]
    public int? InactiveTimeout { get; set; }
}
