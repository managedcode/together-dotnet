using System.Net.Http.Headers;
using Together.Models.Files;

namespace Together.Clients;

public class FileClient(HttpClient httpClient) : BaseClient(httpClient)
{
    public async Task<FileResponse> UploadAsync(
        string filePath, 
        FilePurpose? purpose = null, 
        bool checkFile = true, 
        CancellationToken cancellationToken = default)
    {
        purpose ??= FilePurpose.FineTune;
        
        if (checkFile && !File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found", filePath);
        }

        using var form = new MultipartFormDataContent();
        using var fileStream = File.OpenRead(filePath);
        using var content = new StreamContent(fileStream);
        
        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        form.Add(content, "file", Path.GetFileName(filePath));
        form.Add(new StringContent(purpose.ToString().ToLowerInvariant()), "purpose");

        return await SendRequestAsync<FileResponse>("/files", HttpMethod.Post, form, cancellationToken);
    }

    public async Task<FileList> ListAsync(CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<FileList>("/files", HttpMethod.Get, null, cancellationToken);
    }

    public async Task<FileResponse> RetrieveAsync(string fileId, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<FileResponse>($"/files/{fileId}", HttpMethod.Get, null, cancellationToken);
    }

    public async Task<FileObject> RetrieveContentAsync(string fileId, string? outputPath = null, CancellationToken cancellationToken = default)
    {
        var fileName = outputPath ?? NormalizeKey($"{fileId}.jsonl");
        var response = await HttpClient.GetAsync($"/files/{fileId}/content", cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var fs = File.Create(fileName);
        await response.Content.CopyToAsync(fs, cancellationToken);
        
        var fileInfo = new FileInfo(fileName);
        return new FileObject
        {
            Object = "local",
            Id = fileId,
            Filename = fileName,
            Size = (int)fileInfo.Length
        };
    }

    public async Task<FileDeleteResponse> DeleteAsync(string fileId, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<FileDeleteResponse>($"/files/{fileId}", HttpMethod.Delete, null, cancellationToken);
    }

    private static string NormalizeKey(string key) => string.Join("_", key.Split(Path.GetInvalidFileNameChars()));
}
