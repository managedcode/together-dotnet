using System.Net;
using System.Text;
using Microsoft.Extensions.AI;
using Moq;
using Moq.Protected;
using Together.Clients;
using Together.Models.ChatCompletions;

namespace Together.Tests.Clients;

public class ChatCompletionClientTests : TestBase
{
    [Fact]
    public async Task CreateAsync_SuccessfulResponse_ReturnsChatCompletionResponse()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{
                ""id"": ""test-id"",
                ""object"": ""chat.completion"",
                ""created"": 1234567890,
                ""model"": ""test-model"",
                ""choices"": [{
                    ""index"": 0,
                    ""message"": {
                        ""role"": ""assistant"",
                        ""content"": ""Test response""
                    },
                    ""finish_reason"": ""stop""
                }]
            }")
        };

        var client = new ChatCompletionClient(CreateMockHttpClient(response));
        var request = new ChatCompletionRequest
        {
            Model = "test-model",
            Messages = new List<ChatCompletionMessage>
            {
                new() { Role = ChatRole.User, Content = "Test prompt" }
            }
        };

        // Act
        var result = await client.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test-id", result.Id);
        Assert.Equal("Test response", result.Choices[0].Message.Content);
    }

    [Fact]
    public async Task CreateStreamAsync_SuccessfulResponse_YieldsChunks()
    {
        // Arrange
        var streamContent = """
            data: {"id":"1","object":"chat.completion.chunk","choices":[{"delta":{"role":"assistant"},"index":0}]}
            data: {"id":"1","object":"chat.completion.chunk","choices":[{"delta":{"content":"Hello"},"index":0}]}
            data: {"id":"1","object":"chat.completion.chunk","choices":[{"delta":{"content":" world"},"index":0}]}
            data: [DONE]
            """;

        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(streamContent)
        };

        var client = new ChatCompletionClient(CreateMockHttpClient(response));
        var request = new ChatCompletionRequest
        {
            Model = "test-model",
            Messages = new List<ChatCompletionMessage>
            {
                new() { Role = ChatRole.User, Content = "Test prompt" }
            },
            Stream = true
        };

        // Act
        var chunks = new List<ChatCompletionChunk>();
        await foreach (var chunk in client.CreateStreamAsync(request))
        {
            chunks.Add(chunk);
        }

        // Assert
        Assert.Equal(3, chunks.Count);
        Assert.Equal("Hello", chunks[1].Choices[0].Delta?.Content);
        Assert.Equal(" world", chunks[2].Choices[0].Delta?.Content);
    }
}
