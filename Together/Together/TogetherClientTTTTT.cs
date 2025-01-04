namespace Together.Models.Common;

public class TogetherClientTTTTT
{
    public string ApiKey { get; set; } 
    public string BaseUrl { get; set; } = TogetherConstants.BASE_URL;
    public float Timeout { get; set; } = TogetherConstants.TIMEOUT_SECS;
    public int MaxRetries { get; set; } = TogetherConstants.MAX_RETRIES;
    public Dictionary<string, string> SuppliedHeaders { get; set; } 
}