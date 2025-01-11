using System.Text.Json.Serialization;

namespace Together.Models.Error;

public class ErrorResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("error")]
    public Error Error { get; set; }
}