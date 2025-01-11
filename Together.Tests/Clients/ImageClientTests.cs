using System.Net;
using Together.Clients;
using Together.Models.Images;

namespace Together.Tests.Clients;

public class ImageClientTests : TestBase
{
    [Fact]
    public async Task GenerateAsync_SuccessfulResponse_ReturnsImageResponse()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{
                ""created"": 1234567890,
                ""data"": [{
                    ""url"": ""https://example.com/image.png""
                }]
            }")
        };

        var client = new ImageClient(CreateMockHttpClient(response));
        var request = new ImageRequest
        {
            Model = "test-model",
            Prompt = "Test prompt"
        };

        // Act
        var result = await client.GenerateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Data);
        Assert.Equal("https://example.com/image.png", result.Data[0].Url);
    }
}