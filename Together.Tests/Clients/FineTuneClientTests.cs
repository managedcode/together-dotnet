using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Together.Clients;
using Together.Models.Finetune;
using Xunit;

namespace Together.Tests.Clients;

public class FineTuneClientTests : TestBase
{
    private const string SampleResponse = @"{
        ""id"": ""ft-test-id"",
        ""object"": ""fine-tune"",
        ""model"": ""test-model"",
        ""created_at"": ""2024-01-01T00:00:00Z"",
        ""status"": ""created"",
        ""training_file"": ""file-123"",
        ""training_type"": {
            ""type"": ""lora""
        }
    }";

    [Fact]
    public async Task CreateAsync_SuccessfulResponse_ReturnsFinetuneResponse()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(SampleResponse)
        };

        var client = new FineTuneClient(CreateMockHttpClient(response));
        var request = new FinetuneRequest
        {
            Model = "test-model",
            TrainingFile = "file-123"
        };

        // Act
        var result = await client.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("ft-test-id", result.Id);
        Assert.Equal("created", result.Status.ToString());
    }

    [Fact]
    public async Task ListAsync_SuccessfulResponse_ReturnsFinetuneList()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{
                ""object"": ""list"",
                ""data"": [" + SampleResponse + @"]
            }")
        };

        var client = new FineTuneClient(CreateMockHttpClient(response));

        // Act
        var result = await client.ListAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Data);
        Assert.Equal("ft-test-id", result.Data[0].Id);
    }

    [Fact]
    public async Task RetrieveAsync_SuccessfulResponse_ReturnsFinetuneResponse()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(SampleResponse.Replace("\"created\"", "\"succeeded\""))
        };

        var client = new FineTuneClient(CreateMockHttpClient(response));

        // Act
        var result = await client.RetrieveAsync("ft-test-id");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("ft-test-id", result.Id);
        Assert.Equal("succeeded", result.Status.ToString());
    }

    [Fact]
    public async Task CancelAsync_SuccessfulResponse_ReturnsFinetuneResponse()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(SampleResponse.Replace("\"created\"", "\"cancelled\""))
        };

        var client = new FineTuneClient(CreateMockHttpClient(response));

        // Act
        var result = await client.CancelAsync("ft-test-id");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("ft-test-id", result.Id);
        Assert.Equal("cancelled", result.Status.ToString());
    }

    [Fact]
    public async Task ListEventsAsync_SuccessfulResponse_ReturnsFinetuneListEvents()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{
                ""object"": ""list"",
                ""data"": [{
                    ""object"": ""fine-tune-event"",
                    ""created_at"": ""2024-01-01T00:00:00Z"",
                    ""level"": ""info"",
                    ""message"": ""Test event message""
                }]
            }")
        };

        var client = new FineTuneClient(CreateMockHttpClient(response));

        // Act
        var result = await client.ListEventsAsync("ft-test-id");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Data);
        Assert.Equal("info", result.Data[0].Level.ToString());
        Assert.Equal("Test event message", result.Data[0].Message);
    }

    [Fact]
    public async Task GetModelLimitsAsync_SuccessfulResponse_ReturnsFinetuneTrainingLimits()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{
                ""max_num_epochs"": 100,
                ""max_learning_rate"": 0.002,
                ""min_learning_rate"": 0.0001,
                ""full_training"": {
                    ""max_batch_size"": 32,
                    ""min_batch_size"": 1
                },
                ""lora_training"": {
                    ""max_rank"": 64
                }
            }")
        };

        var client = new FineTuneClient(CreateMockHttpClient(response));

        // Act
        var result = await client.GetModelLimitsAsync("test-model");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(100, result.MaxNumEpochs);
        Assert.Equal(0.002f, result.MaxLearningRate);
        Assert.Equal(0.0001f, result.MinLearningRate);
        Assert.NotNull(result.FullTraining);
        Assert.Equal(32, result.FullTraining.MaxBatchSize);
        Assert.Equal(1, result.FullTraining.MinBatchSize);
        Assert.NotNull(result.LoraTraining);
        Assert.Equal(64, result.LoraTraining.MaxRank);
    }

    [Fact]
    public async Task DownloadAsync_SuccessfulResponse_ReturnsFinetuneDownloadResult()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            // First request - retrieve fine-tune info
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(SampleResponse)
            })
            // Second request - download content
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("model content")
            });

        var httpClient = new HttpClient(mockHandler.Object)
        {
            BaseAddress = new Uri(TogetherConstants.BASE_URL)
        };
        var client = new FineTuneClient(httpClient);

        // Act
        var result = await client.DownloadAsync(
            id: "ft-test-id",
            outputPath: "test-output.bin",
            checkpointStep: 1,
            checkpointType: DownloadCheckpointType.Merged);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("ft-test-id", result.Id);
        Assert.Equal("test-output.bin", result.Filename);
        Assert.Equal(1, result.CheckpointStep);

        // Cleanup
        if (File.Exists("test-output.bin"))
        {
            File.Delete("test-output.bin");
        }
    }
}
