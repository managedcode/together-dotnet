using System.Text.Json.Serialization;

namespace Together.Models.Files;

public class FileRequest
{
    [JsonPropertyName("training_file")]
    public string TrainingFile { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("n_epochs")]
    public int NEpochs { get; set; }

    [JsonPropertyName("learning_rate")]
    public float LearningRate { get; set; }

    [JsonPropertyName("n_checkpoints")]
    public int? NCheckpoints { get; set; }

    [JsonPropertyName("batch_size")]
    public int? BatchSize { get; set; }

    [JsonPropertyName("suffix")]
    public string Suffix { get; set; }

    [JsonPropertyName("wandb_api_key")]
    public string WandbApiKey { get; set; }
}