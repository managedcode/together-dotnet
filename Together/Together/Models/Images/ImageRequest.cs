namespace Together.Models.Images;

public class ImageRequest
{
    public string Prompt { get; set; }
    public string Model { get; set; }
    public int? Steps { get; set; } = 20;
    public int? Seed { get; set; } 
    public int? N { get; set; } = 1;
    public int? Height { get; set; } = 1024;
    public int? Width { get; set; } = 1024;
    public string NegativePrompt { get; set; } 
}