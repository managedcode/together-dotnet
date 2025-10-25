using System.Net.Http;
using System.Net.Http.Json;
using Together.Models.Batch;

namespace Together.Clients;

public class BatchClient(HttpClient httpClient) : BaseClient(httpClient)
{
    public async Task<BatchJob> CreateAsync(BatchCreateRequest request, CancellationToken cancellationToken = default)
    {
        var response = await SendRequestAsync<BatchCreateRequest, BatchCreateResponse>("/batches", request, cancellationToken);
        if (response.Job is null)
        {
            throw new InvalidOperationException("Batch job response did not include job details.");
        }

        return response.Job;
    }

    public async Task<BatchJob> RetrieveAsync(string batchId, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<BatchJob>($"/batches/{batchId}", HttpMethod.Get, null, cancellationToken);
    }

    public async Task<IReadOnlyList<BatchJob>> ListAsync(CancellationToken cancellationToken = default)
    {
        var jobs = await SendRequestAsync<List<BatchJob>>("/batches", HttpMethod.Get, null, cancellationToken);
        return jobs;
    }

    public async Task<BatchJob> CancelAsync(string batchId, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<BatchJob>($"/batches/{batchId}/cancel", HttpMethod.Post, null, cancellationToken);
    }
}
