using Together.Models.Common;

namespace Together.Models.Finetune;

public class FinetuneEvent
{
    public ObjectType Object { get; set; }
    public string CreatedAt { get; set; }
    public FinetuneEventLevels? Level { get; set; }
    public string Message { get; set; }
    public FinetuneEventType? Type { get; set; }
    public int? ParamCount { get; set; }
    public int? TokenCount { get; set; }
    public string WandbUrl { get; set; }
    public string Hash { get; set; }
}