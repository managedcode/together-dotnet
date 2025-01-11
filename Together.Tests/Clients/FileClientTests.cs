using System.Net;
using Moq;
using Moq.Protected;
using Together.Clients;

namespace Together.Tests.Clients;

public class FileClientTests : TestBase
{
    [Fact]
    public async Task UploadAsync_ValidFile_ReturnsFileResponse()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{
                ""id"": ""file-123"",
                ""object"": ""file"",
                ""filename"": ""test.jsonl"",
                ""purpose"": ""fine-tune"",
                ""bytes"": 1234,
                ""created_at"": 1677649421,
                ""status"": ""uploaded""
            }")
        };

        var client = new FileClient(CreateMockHttpClient(response));
        var tempFile = Path.GetTempFileName();
        await File.WriteAllTextAsync(tempFile, "test content");

        try
        {
            // Act
            var result = await client.UploadAsync(tempFile);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("file-123", result.Id);
            Assert.Equal("test.jsonl", result.Filename);
            Assert.Equal("fine-tune", result.Purpose.ToString());
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public async Task ListAsync_ReturnsFileList()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{
                ""object"": ""list"",
                ""data"": [{
                    ""id"": ""file-123"",
                    ""object"": ""file"",
                    ""filename"": ""test.jsonl"",
                    ""purpose"": ""fine-tune"",
                    ""bytes"": 1234,
                    ""created_at"": 1677649421,
                    ""status"": ""uploaded""
                }]
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

    [Fact]
    public async Task RetrieveAsync_ValidFileId_ReturnsFileResponse()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{
                ""id"": ""file-123"",
                ""object"": ""file"",
                ""filename"": ""test.jsonl"",
                ""purpose"": ""fine-tune"",
                ""bytes"": 1234,
                ""created_at"": 1677649421,
                ""status"": ""uploaded""
            }")
        };

        var client = new FileClient(CreateMockHttpClient(response));

        // Act
        var result = await client.RetrieveAsync("file-123");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("file-123", result.Id);
        Assert.Equal("test.jsonl", result.Filename);
    }

    [Fact]
    public async Task RetrieveContentAsync_ValidFileId_SavesContentAndReturnsFileObject()
    {
        // Arrange
        var fileContent = "test file content";
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(fileContent)
        };

        var client = new FileClient(CreateMockHttpClient(response));
        var tempFile = Path.Combine(Path.GetTempPath(), "file-123.jsonl");

        try
        {
            // Act
            var result = await client.RetrieveContentAsync("file-123", tempFile);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("file-123", result.Id);
            Assert.Equal(tempFile, result.Filename);
            Assert.True(File.Exists(tempFile));
            Assert.Equal(fileContent, await File.ReadAllTextAsync(tempFile));
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }

    [Fact]
    public async Task DeleteAsync_ValidFileId_ReturnsDeleteResponse()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{
                ""id"": ""file-123"",
                ""object"": ""file"",
                ""deleted"": true
            }")
        };

        var client = new FileClient(CreateMockHttpClient(response));

        // Act
        var result = await client.DeleteAsync("file-123");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("file-123", result.Id);
        Assert.True(result.Deleted);
    }

    [Fact]
    public async Task UploadAsync_FileNotFound_ThrowsFileNotFoundException()
    {
        // Arrange
        var client = new FileClient(new HttpClient());
        var nonExistentFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid()
            .ToString());

        // Act & Assert
        await Assert.ThrowsAsync<FileNotFoundException>(() => client.UploadAsync(nonExistentFile));
    }

    [Fact]
    public async Task UploadAsync_NetworkError_ThrowsException()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        var client = new FileClient(new HttpClient(mockHandler.Object));
        var tempFile = Path.GetTempFileName();

        try
        {
            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => client.UploadAsync(tempFile));
        }
        finally
        {
            File.Delete(tempFile);
        }
    }
}