using System.Text.Json.Serialization;

namespace Together.Models.Files;

public class FileList
{
    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("data")]
    public List<FileResponse> Data { get; set; }
}