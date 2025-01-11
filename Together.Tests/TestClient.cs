using Together.Clients;

namespace Together.Tests;

public class TestClient(HttpClient httpClient) : BaseClient(httpClient)
{
    public Task<TResponse> TestSendRequest<TRequest, TResponse>(string uri, TRequest request, CancellationToken ct = default)
    {
        return SendRequestAsync<TRequest, TResponse>(uri, request, ct);
    }
}