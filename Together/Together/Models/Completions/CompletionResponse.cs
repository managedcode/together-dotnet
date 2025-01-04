using Together.Models.Common;

namespace Together.Models.Completions;

public class CompletionResponse
{
    public string Id { get; set; }
    public ObjectType Object { get; set; }
    public int? Created { get; set; }
    public string Model { get; set; }
    public List<CompletionChoicesData> Choices { get; set; }
    public List<PromptPart> Prompt { get; set; }
    public UsageData Usage { get; set; }
}