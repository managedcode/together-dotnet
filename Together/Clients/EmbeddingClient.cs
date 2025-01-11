using Together.Models.Embeddings;

namespace Together.Clients;

public class EmbeddingClient(HttpClient httpClient) : BaseClient(httpClient)
{
    public async Task<EmbeddingResponse> CreateAsync(EmbeddingRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<EmbeddingRequest, EmbeddingResponse>("/embeddings", request, cancellationToken);
    }
}