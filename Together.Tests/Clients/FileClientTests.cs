using System.Net;
using Moq;
using Moq.Protected;
using Together.Clients;
using Together.Models.Files;

namespace Together.Tests.Clients;

public class FileClientTests : TestBase
{
    private readonly string _testFilePath = Path.GetTempFileName();

    public FileClientTests()
    {
        File.WriteAllText(_testFilePath, "test content");
    }

    [Fact]
    public async Task UploadAsync_SuccessfulResponse_ReturnsFileResponse()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{
                ""id"": ""file-123"",
                ""object"": ""file"",
                ""bytes"": 1234,
                ""created_at"": 1234567890,
                ""filename"": ""test.jsonl"",
                ""purpose"": ""fine-tune""
            }")
        };

        var client = new FileClient(CreateMockHttpClient(response));

        // Act
        var result = await client.UploadAsync(_testFilePath);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("file-123", result.Id);
        Assert.Equal("test.jsonl", result.Filename);
    }

    [Fact]
    public async Task ListAsync_SuccessfulResponse_ReturnsFileList()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{
                ""data"": [{
                    ""id"": ""file-123"",
                    ""object"": ""file"",
                    ""bytes"": 1234,
                    ""created_at"": 1234567890,
                    ""filename"": ""test.jsonl"",
                    ""purpose"": ""fine-tune""
                }],
                ""object"": ""list""
            }")
        };

        var client = new FileClient(CreateMockHttpClient(response));

        // Act
        var result = await client.ListAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Data);
        Assert.Equal("file-123", result.Data[0].Id);
    }
    
    public void Dispose()
    {
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }
}
