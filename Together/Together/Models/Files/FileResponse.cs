using Together.Models.Common;

namespace Together.Models.Files;

public class FileResponse
{
    public string Id { get; set; }
    public ObjectType Object { get; set; }
    public int? CreatedAt { get; set; }
    public FileType? Type { get; set; }
    public FilePurpose? Purpose { get; set; }
    public string Filename { get; set; }
    public int? Bytes { get; set; }
    public int? LineCount { get; set; }
    public bool? Processed { get; set; }
}