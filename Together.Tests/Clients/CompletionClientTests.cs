using System.Net;
using Moq;
using Moq.Protected;
using Together.Clients;
using Together.Models.Completions;

namespace Together.Tests.Clients;

public class CompletionClientTests : TestBase
{
    [Fact]
    public async Task CreateAsync_SuccessfulResponse_ReturnsCompletionResponse()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{
                ""id"": ""test-id"",
                ""object"": ""text_completion"",
                ""created"": 1234567890,
                ""model"": ""test-model"",
                ""choices"": [{
                    ""text"": ""Test response"",
                    ""index"": 0,
                    ""logprobs"": null,
                    ""finish_reason"": ""stop""
                }]
            }")
        };

        var client = new CompletionClient(CreateMockHttpClient(response));
        var request = new CompletionRequest
        {
            Model = "test-model",
            Prompt = "Test prompt"
        };

        // Act
        var result = await client.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test-id", result.Id);
        Assert.Equal("Test response", result.Choices[0].Text);
    }
}
