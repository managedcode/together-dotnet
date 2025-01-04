using System.Text.Json.Serialization;

namespace Together.Models.Error;

public class Error
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("param")]
    public string Param { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }
}