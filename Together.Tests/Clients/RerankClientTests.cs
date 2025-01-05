using System.Net;
using Moq;
using Moq.Protected;
using Together.Clients;
using Together.Models.Rerank;

namespace Together.Tests.Clients;

public class RerankClientTests : TestBase
{
    [Fact]
    public async Task CreateAsync_SuccessfulResponse_ReturnsRerankResponse()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{
                ""results"": [{
                    ""index"": 0,
                    ""relevance_score"": 0.95,
                    ""document"": ""Test document""
                }]
            }")
        };

        var client = new RerankClient(CreateMockHttpClient(response));
        var request = new RerankRequest
        {
            Model = "test-model",
            Query = "test query",
            Documents = new List<string> { "Test document" }
        };

        // Act
        var result = await client.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Results);
        Assert.Equal(0.95, result.Results[0].RelevanceScore);
        Assert.Equal("Test document", result.Results[0].Document.First());
    }
}
