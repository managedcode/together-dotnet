using Together.Models.Images;

namespace Together.Clients;

public class ImageClient(HttpClient httpClient) : BaseClient(httpClient)
{
    public async Task<ImageResponse> GenerateAsync(ImageRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<ImageRequest, ImageResponse>("/images/generations", request, cancellationToken);
    }
}