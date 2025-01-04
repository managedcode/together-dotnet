namespace Together.Models.Error;

public class TogetherErrorResponse
{
    // Error message
    public string Message { get; set; }

    // Error type
    public string Type { get; set; }

    // Param causing error
    public string Param { get; set; }

    // Error code
    public string Code { get; set; }
}