using System.Net;
using Together.Clients;
using Together.Models.Embeddings;

namespace Together.Tests.Clients;

public class EmbeddingClientTests : TestBase
{
    [Fact]
    public async Task CreateAsync_SuccessfulResponse_ReturnsEmbeddingResponse()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{
                ""object"": ""list"",
                ""data"": [{
                    ""object"": ""embedding"",
                    ""embedding"": [0.1, 0.2, 0.3],
                    ""index"": 0
                }],
                ""model"": ""test-model"",
                ""usage"": {
                    ""prompt_tokens"": 10,
                    ""total_tokens"": 10
                }
            }")
        };

        var client = new EmbeddingClient(CreateMockHttpClient(response));
        var request = new EmbeddingRequest
        {
            Model = "test-model",
            Input = "Test input"
        };

        // Act
        var result = await client.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Data);
        Assert.Equal(3, result.Data[0].Embedding.Count);
        Assert.Equal(0.1f, result.Data[0]
            .Embedding[0]);
    }
}