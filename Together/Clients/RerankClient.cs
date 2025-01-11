using Together.Models.Rerank;

namespace Together.Clients;

public class RerankClient(HttpClient httpClient) : BaseClient(httpClient)
{
    public async Task<RerankResponse> CreateAsync(RerankRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<RerankRequest, RerankResponse>("/rerank", request, cancellationToken);
    }
}