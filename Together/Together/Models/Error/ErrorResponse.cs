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


public class ErrorResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("error")]
    public Error Error { get; set; }
}