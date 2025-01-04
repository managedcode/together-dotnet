using System.Text.Json.Serialization;
using Microsoft.Extensions.AI;
using System.Collections.Generic;

namespace Together.Models.Common;

public class TogetherRequest
{
    [JsonPropertyName("method")]
    public string Method { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("headers")]
    public Dictionary<string, string> Headers { get; set; } = new();

    [JsonPropertyName("params")]
    public object Params { get; set; }

    [JsonPropertyName("files")]
    public Dictionary<string, object> Files { get; set; } = new();

    [JsonPropertyName("allow_redirects")]
    public bool AllowRedirects { get; set; } = true;

    [JsonPropertyName("override_headers")]
    public bool OverrideHeaders { get; set; } = false;
}