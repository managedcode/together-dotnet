using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.AI;
using Together.Clients;
using Together.Extensions.MicrosoftAI;

namespace Together.Tests.MicrosoftAI;

public class MicrosoftAIAdapterTests : TestBase
{
    [Fact]
    public async Task ChatAdapter_ConvertsResponse()
    {
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(
                """{"id":"resp","model":"chat-model","choices":[{"message":{"role":"assistant","content":"Hello"},"finish_reason":"stop"}],"usage":{"prompt_tokens":1,"completion_tokens":2,"total_tokens":3}}""")
        };

        var adapter = new TogetherAIChatClient(new ChatCompletionClient(CreateMockHttpClient(response)), "chat-model");
        var chatResponse = await adapter.GetResponseAsync(new[] { new ChatMessage(ChatRole.User, "Hi") }, new ChatOptions(), CancellationToken.None);

        Assert.Equal("Hello", chatResponse.Text);
        Assert.Equal("chat-model", chatResponse.ModelId);
        Assert.NotNull(chatResponse.Usage);
    }

    [Fact]
    public async Task ChatAdapter_StreamsUpdates()
    {
        var payload = "data: {\"id\":\"chunk\",\"model\":\"chat-model\",\"choices\":[{\"delta\":{\"content\":\"He\"}}]}\n" +
                      "data: {\"id\":\"chunk\",\"model\":\"chat-model\",\"choices\":[{\"delta\":{\"content\":\"llo\"}}]}\n" +
                      "data: [DONE]\n";
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(payload)
        };

        var adapter = new TogetherAIChatClient(new ChatCompletionClient(CreateMockHttpClient(response)), "chat-model");
        var updates = new List<ChatResponseUpdate>();
        await foreach (var update in adapter.GetStreamingResponseAsync(new[] { new ChatMessage(ChatRole.User, "Hi") }, new ChatOptions(), CancellationToken.None))
        {
            updates.Add(update);
        }

        Assert.Equal("Hello", string.Concat(updates.Select(u => u.Text)));
    }

    [Fact]
    public async Task EmbeddingAdapter_GeneratesEmbeddings()
    {
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(
                """{"id":"emb","model":"embed-model","object":"list","data":[{"index":0,"object":"embedding","embedding":[0.1,0.2]}]}""")
        };

        var adapter = new TogetherAIEmbeddingGenerator(new EmbeddingClient(CreateMockHttpClient(response)), "embed-model");
        var result = await adapter.GenerateAsync(new[] { "test" }, new EmbeddingGenerationOptions(), CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(2, result.First().Vector.Length);
    }

    [Fact]
    public async Task SpeechToTextAdapter_ReturnsText()
    {
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("""{"text":"transcribed"}""")
        };

        var adapter = new TogetherAISpeechToTextClient(new AudioClient(CreateMockHttpClient(response)), "whisper");
        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes("audio"));
        var result = await adapter.GetTextAsync(stream, new SpeechToTextOptions(), CancellationToken.None);

        Assert.Equal("transcribed", result.Text);
    }

    [Fact]
    public async Task ImageAdapter_ReturnsImageContent()
    {
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(
                """{"id":"img","model":"image-model","object":"list","data":[{"index":0,"url":"https://example.com/image.png"}]}""")
        };

        var adapter = new TogetherAIImageClient(new ImageClient(CreateMockHttpClient(response)), "image-model");
        var chatResponse = await adapter.GetResponseAsync(new[] { new ChatMessage(ChatRole.User, "make image") }, new ChatOptions(), CancellationToken.None);

        Assert.Single(chatResponse.Messages.First().Contents);
        Assert.IsType<UriContent>(chatResponse.Messages.First().Contents[0]);
    }
}
