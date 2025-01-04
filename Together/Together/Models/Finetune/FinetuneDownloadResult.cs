namespace Together.Models.Finetune;

public class FinetuneDownloadResult
{
    public string Object { get; set; }
    public string Id { get; set; }
    public int? CheckpointStep { get; set; }
    public string Filename { get; set; }
    public int? Size { get; set; }
}