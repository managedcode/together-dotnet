using System.Net;
using Together.Clients;

namespace Together.Tests.Clients;

public class ModelClientTests : TestBase
{
    [Fact]
    public async Task ListModelsAsync_SuccessfulResponse_ReturnsModelList()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"[
                {
                    ""id"": ""model-1"",
                    ""name"": ""Test Model 1"",
                    ""description"": ""Test Description"",
                    ""context_length"": 2048,
                    ""token_limit"": 2048
                },
                {
                    ""id"": ""model-2"",
                    ""name"": ""Test Model 2"",
                    ""description"": ""Test Description 2"",
                    ""context_length"": 4096,
                    ""token_limit"": 4096
                }
            ]")
        };

        var client = new ModelClient(CreateMockHttpClient(response));

        // Act
        var result = await client.ListModelsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("model-1", result[0].Id);
        Assert.Equal("model-2", result[1].Id);
    }
}