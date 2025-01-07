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

    [Fact]
    public async Task CreateAsync_WithFunctionCalling_ReturnsToolCalls()
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
                        ""tool_calls"": [{
                            ""id"": ""call_123"",
                            ""type"": ""function"",
                            ""function"": {
                                ""name"": ""get_current_weather"",
                                ""arguments"": ""{\""location\"":\""New York, NY\"",\""unit\"":\""fahrenheit\""}""
                            }
                        }]
                    },
                    ""finish_reason"": ""tool_calls""
                }]
            }")
        };

        var client = new ChatCompletionClient(CreateMockHttpClient(response));
        var request = new ChatCompletionRequest
        {
            Model = "mistralai/Mixtral-8x7B-Instruct-v0.1",
            Messages = new List<ChatCompletionMessage>
            {
                new() { Role = ChatRole.User, Content = "What's the weather in New York?" }
            },
            Tools = new List<Tool>
            {
                new()
                {
                    Type = "function",
                    Function = new FunctionTool
                    {
                        Name = "get_current_weather",
                        Description = "Get the current weather",
                        Parameters = new Dictionary<string, object>
                        {
                            ["type"] = "object",
                            ["properties"] = new Dictionary<string, object>
                            {
                                ["location"] = new Dictionary<string, object> { ["type"] = "string" },
                                ["unit"] = new Dictionary<string, object>
                                {
                                    ["type"] = "string",
                                    ["enum"] = new[] { "celsius", "fahrenheit" }
                                }
                            }
                        }
                    }
                }
            }
        };

        // Act
        var result = await client.CreateAsync(request);

        // Assert
        Assert.NotNull(result.Choices[0].Message.ToolCalls);
        Assert.Equal("get_current_weather", result.Choices[0].Message.ToolCalls[0].Function.Name);
        Assert.Contains("New York", result.Choices[0].Message.ToolCalls[0].Function.Arguments);
    }

    [Fact]
    public async Task CreateAsync_WithSpecificToolChoice_UsesRequestedFunction()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{
                ""id"": ""test-id"",
                ""choices"": [{
                    ""index"": 0,
                    ""message"": {
                        ""role"": ""assistant"",
                        ""tool_calls"": [{
                            ""id"": ""call_123"",
                            ""type"": ""function"",
                            ""function"": {
                                ""name"": ""get_current_weather"",
                                ""arguments"": ""{\""location\"":\""Chicago, IL\""}""
                            }
                        }]
                    }
                }]
            }")
        };

        var client = new ChatCompletionClient(CreateMockHttpClient(response));
        var request = new ChatCompletionRequest
        {
            Model = "mistralai/Mixtral-8x7B-Instruct-v0.1",
            Messages = new List<ChatCompletionMessage>
            {
                new() { Role = ChatRole.User, Content = "Check the weather" }
            },
            Tools = new List<Tool>
            {
                new()
                {
                    Type = "function",
                    Function = new FunctionTool
                    {
                        Name = "get_current_weather",
                        Description = "Get the current weather",
                        Parameters = new Dictionary<string, object>
                        {
                            ["type"] = "object",
                            ["properties"] = new Dictionary<string, object>
                            {
                                ["location"] = new Dictionary<string, object> { ["type"] = "string" }
                            },
                            ["required"] = new[] { "location" }
                        }
                    }
                }
            },
            ToolChoice = new ToolChoice
            {
                Type = "function",
                Function = new FunctionToolChoice { Name = "get_current_weather" }
            }
        };

        // Act
        var result = await client.CreateAsync(request);

        // Assert
        Assert.Equal("get_current_weather", result.Choices[0].Message.ToolCalls[0].Function.Name);
    }

    [Fact]
    public async Task CreateAsync_InvalidModel_ThrowsException()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest,
            Content = new StringContent(@"{
                ""error"": {
                    ""message"": ""Invalid model name"",
                    ""type"": ""invalid_request_error""
                }
            }")
        };

        var client = new ChatCompletionClient(CreateMockHttpClient(response));
        var request = new ChatCompletionRequest
        {
            Model = "invalid-model",
            Messages = new List<ChatCompletionMessage>
            {
                new() { Role = ChatRole.User, Content = "Test prompt" }
            }
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => client.CreateAsync(request));
    }

    [Fact]
    public async Task CreateAsync_NetworkError_ThrowsException()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        var client = new ChatCompletionClient(new HttpClient(mockHandler.Object));
        var request = new ChatCompletionRequest
        {
            Model = "test-model",
            Messages = new List<ChatCompletionMessage>
            {
                new() { Role = ChatRole.User, Content = "Test prompt" }
            }
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => client.CreateAsync(request));
    }
}
