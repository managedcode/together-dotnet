using Together.Models.Common;

namespace Together.Models.Models;

public class ModelObject
{
    public string Id { get; set; }
    public ObjectType Object { get; set; }
    public int? Created { get; set; }
    public ModelType? Type { get; set; }
    public string DisplayName { get; set; }
    public string Organization { get; set; }
    public string Link { get; set; }
    public string License { get; set; }
    public int? ContextLength { get; set; }
    public PricingObject Pricing { get; set; }
}