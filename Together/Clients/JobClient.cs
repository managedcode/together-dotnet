using System.Net.Http;
using Together.Models.Jobs;

namespace Together.Clients;

public class JobClient(HttpClient httpClient) : BaseClient(httpClient)
{
    public async Task<JobRetrieveResponse> RetrieveAsync(string jobId, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<JobRetrieveResponse>($"/jobs/{jobId}", HttpMethod.Get, null, cancellationToken);
    }

    public async Task<JobListResponse> ListAsync(CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<JobListResponse>("/jobs", HttpMethod.Get, null, cancellationToken);
    }
}
