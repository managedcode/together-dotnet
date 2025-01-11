using Moq;
using Moq.Protected;

namespace Together.Tests;

public abstract class TestBase
{
    protected static HttpClient CreateMockHttpClient(HttpResponseMessage response)
    {
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var client = new HttpClient(mockHandler.Object);
        client.BaseAddress = new Uri(TogetherConstants.BASE_URL);
        return client;
    }
}