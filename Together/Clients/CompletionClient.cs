using Together.Models.Completions;

namespace Together.Clients;

public class CompletionClient(HttpClient httpClient) : BaseClient(httpClient)
{
    public async Task<CompletionResponse> CreateAsync(CompletionRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<CompletionRequest, CompletionResponse>("/completions", request, cancellationToken);
    }
}