using Together.Models.Common;

namespace Together.Models.Completions;

public class CompletionChunk
{
    public string Id { get; set; }
    public ObjectType Object { get; set; }
    public int? Created { get; set; }
    public string Model { get; set; }
    public List<CompletionChoicesChunk> Choices { get; set; }
    public UsageData Usage { get; set; }
}