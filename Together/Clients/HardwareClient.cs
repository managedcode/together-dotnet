using System;
using System.Net.Http;
using Together.Models.Endpoints;

namespace Together.Clients;

public class HardwareClient(HttpClient httpClient) : BaseClient(httpClient)
{
    public async Task<HardwareListResponse> ListAsync(string? model = null, CancellationToken cancellationToken = default)
    {
        var url = "/hardware";
        if (!string.IsNullOrWhiteSpace(model))
        {
            url += $"?model={Uri.EscapeDataString(model)}";
        }

        return await SendRequestAsync<HardwareListResponse>(url, HttpMethod.Get, null, cancellationToken);
    }
}
