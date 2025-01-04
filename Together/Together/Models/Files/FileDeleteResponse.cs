using Together.Models.Common;

namespace Together.Models.Files;

public class FileDeleteResponse
{
    public string Id { get; set; }
    public ObjectType Object { get; set; }
    public bool Deleted { get; set; }
}