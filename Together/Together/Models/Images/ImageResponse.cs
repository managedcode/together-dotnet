namespace Together.Models.Images;

public class ImageResponse
{
    public string Id { get; set; } 
    public string Model { get; set; } 
    public string Object { get; set; }  // Use string to handle Literal["list"]
    public List<ImageChoicesData> Data { get; set; } 
}