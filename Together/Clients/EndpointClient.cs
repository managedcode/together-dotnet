using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using Together.Models.Endpoints;

namespace Together.Clients;

public class EndpointClient(HttpClient httpClient) : BaseClient(httpClient)
{
    public async Task<IReadOnlyList<ListEndpoint>> ListAsync(string? type = null, CancellationToken cancellationToken = default)
    {
        var url = "/endpoints";
        if (!string.IsNullOrWhiteSpace(type))
        {
            url += $"?type={Uri.EscapeDataString(type)}";
        }

        var response = await SendRequestAsync<JsonObject>(url, HttpMethod.Get, null, cancellationToken);
        if (response.TryGetPropertyValue("data", out var dataNode) && dataNode is JsonArray array)
        {
            var results = new List<ListEndpoint>();
            foreach (var item in array)
            {
                if (item is null)
                {
                    continue;
                }

                var model = item.Deserialize<ListEndpoint>();
                if (model != null)
                {
                    results.Add(model);
                }
            }

            return results;
        }

        return Array.Empty<ListEndpoint>();
    }

    public async Task<DedicatedEndpoint> CreateAsync(EndpointCreateRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<EndpointCreateRequest, DedicatedEndpoint>("/endpoints", request, cancellationToken);
    }

    public async Task<DedicatedEndpoint> RetrieveAsync(string endpointId, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<DedicatedEndpoint>($"/endpoints/{endpointId}", HttpMethod.Get, null, cancellationToken);
    }

    public async Task DeleteAsync(string endpointId, CancellationToken cancellationToken = default)
    {
        await SendRequestAsync<HttpResponseMessage>($"/endpoints/{endpointId}", HttpMethod.Delete, null, cancellationToken);
    }
}
