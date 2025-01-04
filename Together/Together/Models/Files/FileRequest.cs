namespace Together.Models.Files;

public class FileRequest
{
    public string TrainingFile { get; set; }
    public string Model { get; set; }
    public int NEpochs { get; set; }
    public float LearningRate { get; set; }
    public int? NCheckpoints { get; set; }
    public int? BatchSize { get; set; }
    public string Suffix { get; set; }
    public string WandbApiKey { get; set; }
}