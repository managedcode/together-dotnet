using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Together.Models.CodeInterpreter;

public class FileInput
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("encoding")]
    public string Encoding { get; set; } = "string";

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}

public class InterpreterOutput
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public object? Data { get; set; }
}

public class ExecuteResponseData
{
    [JsonPropertyName("outputs")]
    public List<InterpreterOutput> Outputs { get; set; } = new();

    [JsonPropertyName("errors")]
    public string? Errors { get; set; }

    [JsonPropertyName("session_id")]
    public string SessionId { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = "completed";
}

public class ExecuteResponse
{
    [JsonPropertyName("data")]
    public ExecuteResponseData Data { get; set; } = new();
}

public class CodeInterpreterRequest
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("language")]
    public string Language { get; set; } = "python";

    [JsonPropertyName("session_id")]
    public string? SessionId { get; set; }

    [JsonPropertyName("files")]
    public List<FileInput>? Files { get; set; }
}
