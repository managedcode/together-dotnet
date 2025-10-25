using System.Net.Http;
using Together.Models.Videos;

namespace Together.Clients;

public class VideoClient(HttpClient httpClient) : BaseClient(httpClient)
{
    public async Task<CreateVideoResponse> CreateAsync(CreateVideoRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<CreateVideoRequest, CreateVideoResponse>("/../v2/videos", request, cancellationToken);
    }

    public async Task<VideoJob> RetrieveAsync(string id, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<VideoJob>($"/../v2/videos/{id}", HttpMethod.Get, null, cancellationToken);
    }
}
