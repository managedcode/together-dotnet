namespace Together.Models.Common;

public class TogetherRequest
{
    public string Method { get; set; }
    public string Url { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new();
    public object Params { get; set; } // Use object to handle both Dictionary and CallbackIOWrapper
    public Dictionary<string, object> Files { get; set; } = new();
    public bool AllowRedirects { get; set; } = true;
    public bool OverrideHeaders { get; set; } = false;
}