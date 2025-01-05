using System.Net;
using Moq;
using Moq.Protected;
using Together.Clients;
using Together.Models.Error;

namespace Together.Tests.Clients;

public class BaseClientTests : TestBase
{

    private record TestRequest(string Data);
    private record TestResponse(string Result);

    [Fact]
    public async Task SendRequestAsync_ThrowsException_WhenErrorResponse()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest,
            Content = new StringContent(@"{""error"":{""message"":""Test error""}}")
        };
        var client = new TestClient(CreateMockHttpClient(response));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            client.TestSendRequest<TestRequest, TestResponse>("/test", new TestRequest("test")));
        Assert.Equal("Test error", exception.Message);
    }

    [Fact]
    public async Task SendRequestAsync_ReturnsResponse_WhenSuccessful()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{""result"":""success""}")
        };
        var client = new TestClient(CreateMockHttpClient(response));

        // Act
        var result = await client.TestSendRequest<TestRequest, TestResponse>("/test", new TestRequest("test"));

        // Assert
        Assert.Equal("success", result.Result);
    }
}
