namespace Together.Models.Common;

public class TogetherResponse(Dictionary<string, object> data, Dictionary<string, object> headers)
{
    public Dictionary<string, object> Data { get; private set; } = data;

    public string? RequestId
    {
        get
        {
            if (headers.TryGetValue("cf-ray", out var header))
            {
                return header.ToString();
            }

            return null;
        }
    }

    public int? RequestsRemaining
    {
        get
        {
            if (headers.TryGetValue("x-ratelimit-remaining", out var header))
            {
                return Convert.ToInt32(header);
            }

            return null;
        }
    }

    public string? ProcessedBy
    {
        get
        {
            if (headers.TryGetValue("x-hostname", out var header))
            {
                return header.ToString();
            }

            return null;
        }
    }

    public int? ResponseMs
    {
        get
        {
            if (headers.TryGetValue("x-total-time", out var h))
            {
                return h == null ? null : Convert.ToInt32(Math.Round(Convert.ToDouble(h)));
            }

            return null;
        }
    }
}