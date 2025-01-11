using System.Net.Http.Json;
using Together.Models.Models;

namespace Together.Clients;

public class ModelClient(HttpClient httpClient) : BaseClient(httpClient)
{
    public async Task<List<ModelObject>> ListModelsAsync(CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync("/models", cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<ModelObject>>(cancellationToken);
        return result ?? new List<ModelObject>();
    }
}