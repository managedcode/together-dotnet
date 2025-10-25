using System;
using System.Collections.Generic;
using System.Net.Http;
using Together.Models.Evaluations;

namespace Together.Clients;

public class EvaluationClient(HttpClient httpClient) : BaseClient(httpClient)
{
    public async Task<EvaluationCreateResponse> CreateAsync(EvaluationCreateRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<EvaluationCreateRequest, EvaluationCreateResponse>("/evaluation", request, cancellationToken);
    }

    public async Task<IReadOnlyList<EvaluationJob>> ListAsync(EvaluationListOptions? options = null, CancellationToken cancellationToken = default)
    {
        var url = "/evaluations";
        if (options is not null)
        {
            var query = new List<string>();
            if (!string.IsNullOrWhiteSpace(options.Status))
            {
                query.Add($"status={Uri.EscapeDataString(options.Status)}");
            }

            if (options.Limit.HasValue)
            {
                query.Add($"limit={options.Limit.Value}");
            }

            if (query.Count > 0)
            {
                url += "?" + string.Join("&", query);
            }
        }

        return await SendRequestAsync<List<EvaluationJob>>(url, HttpMethod.Get, null, cancellationToken);
    }

    public async Task<EvaluationRetrieveResponse> RetrieveAsync(string workflowId, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<EvaluationRetrieveResponse>($"/evaluation/{workflowId}", HttpMethod.Get, null, cancellationToken);
    }

    public async Task<EvaluationStatusResponse> GetStatusAsync(string workflowId, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<EvaluationStatusResponse>($"/evaluation/{workflowId}/status", HttpMethod.Get, null, cancellationToken);
    }

    public async Task<EvaluationAllowedModelsResponse> GetAllowedModelsAsync(CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<EvaluationAllowedModelsResponse>("/evaluations/model-list", HttpMethod.Get, null, cancellationToken);
    }
}
