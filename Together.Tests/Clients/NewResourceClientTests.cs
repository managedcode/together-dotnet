using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Together.Clients;
using Together.Models.Audio;
using Together.Models.Batch;
using Together.Models.CodeInterpreter;
using Together.Models.Evaluations;
using Together.Models.Videos;

namespace Together.Tests.Clients;

public class NewResourceClientTests : TestBase
{
    [Fact]
    public async Task BatchClient_CreateAsync_ReturnsJob()
    {
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(
                """{"job":{"id":"batch_1","input_file_id":"file_1","user_id":"user","file_size_bytes":10,"status":"IN_PROGRESS","job_deadline":"2024-01-01T00:00:00Z","created_at":"2024-01-01T00:00:00Z","endpoint":"/v1/completions","progress":0.5}}""")
        };

        var client = new BatchClient(CreateMockHttpClient(response));
        var result = await client.CreateAsync(new BatchCreateRequest { InputFileId = "file_1", Endpoint = "/v1/completions" });

        Assert.Equal("batch_1", result.Id);
        Assert.Equal("file_1", result.InputFileId);
    }

    [Fact]
    public async Task EndpointClient_ListAsync_ParsesResponse()
    {
        var json = """{"data":[{"id":"ep1","object":"endpoint","name":"ep","model":"model","type":"dedicated","owner":"user","state":"STARTED","created_at":"2024-01-01T00:00:00Z"}]}""";
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(json)
        };

        var client = new EndpointClient(CreateMockHttpClient(response));
        var results = await client.ListAsync();

        Assert.Single(results);
        Assert.Equal("ep1", results[0].Id);
    }

    [Fact]
    public async Task HardwareClient_ListAsync_ReturnsHardware()
    {
        var json = """{"object":"list","data":[{"object":"hardware","id":"hw1","pricing":{"cents_per_minute":1},"specs":{"gpu_type":"A100","gpu_link":"NVLINK","gpu_memory":80,"gpu_count":1},"updated_at":"2024-01-01T00:00:00Z"}]}""";
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(json)
        };

        var client = new HardwareClient(CreateMockHttpClient(response));
        var result = await client.ListAsync();

        Assert.Single(result.Data);
        Assert.Equal("hw1", result.Data[0].Id);
    }

    [Fact]
    public async Task JobClient_RetrieveAsync_ReturnsJob()
    {
        var json = """{"job_id":"job-1","args":{},"created_at":"2024-01-01","status":"Queued","status_updates":[],"type":"train","updated_at":"2024-01-01"}""";
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(json)
        };

        var client = new JobClient(CreateMockHttpClient(response));
        var job = await client.RetrieveAsync("job-1");

        Assert.Equal("job-1", job.JobId);
        Assert.Equal("Queued", job.Status);
    }

    [Fact]
    public async Task EvaluationClient_CreateAsync_ReturnsWorkflow()
    {
        var json = """{"workflow_id":"wf_1","status":"queued"}""";
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(json)
        };

        var client = new EvaluationClient(CreateMockHttpClient(response));
        var result = await client.CreateAsync(new EvaluationCreateRequest
        {
            Type = "classify",
            Judge = new JudgeModelConfig { Model = "judge", ModelSource = "serverless", SystemTemplate = "template" },
            InputDataFilePath = "file.jsonl",
            Labels = new List<string> { "yes" },
            PassLabels = new List<string> { "yes" }
        });

        Assert.Equal("wf_1", result.WorkflowId);
        Assert.Equal("queued", result.Status);
    }

    [Fact]
    public async Task CodeInterpreterClient_RunAsync_ReturnsOutputs()
    {
        var json = """{"data":{"session_id":"sess","status":"completed","outputs":[{"type":"stdout","data":"hello"}]}}""";
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(json)
        };

        var client = new CodeInterpreterClient(CreateMockHttpClient(response));
        var result = await client.RunAsync(new CodeInterpreterRequest { Code = "print('hi')" });

        Assert.Equal("sess", result.Data.SessionId);
        Assert.Single(result.Data.Outputs);
    }

    [Fact]
    public async Task AudioClient_CreateSpeechAsync_ReturnsBytes()
    {
        var audioBytes = new byte[] { 1, 2, 3 };
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new ByteArrayContent(audioBytes)
        };
        response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("audio/wav");

        var client = new AudioClient(CreateMockHttpClient(response));
        var result = await client.CreateSpeechAsync(new AudioSpeechRequest { Model = "model", Input = "hello" });

        Assert.NotNull(result.Data);
        Assert.Equal(audioBytes, result.Data);
    }

    [Fact]
    public async Task AudioClient_CreateSpeechAsync_ParsesStream()
    {
        var payload = "data: {\"b64\":\"AQI=\"}\n" +
                      "data: [DONE]\n";
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(payload)
        };

        var client = new AudioClient(CreateMockHttpClient(response));
        var request = new AudioSpeechRequest { Model = "model", Input = "hi", Stream = true };
        var stream = await client.CreateSpeechAsync(request);

        var chunks = new List<byte[]>();
        await foreach (var chunk in stream.Stream!)
        {
            chunks.Add(chunk);
        }

        Assert.Single(chunks);
        Assert.Equal(new byte[] { 1, 2 }, chunks[0]);
    }

    [Fact]
    public async Task AudioClient_CreateTranscriptionAsync_ReturnsText()
    {
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("""{"text":"Hello"}""")
        };

        var request = new AudioFileRequest
        {
            Content = new MemoryStream(Encoding.UTF8.GetBytes("data")),
            FileName = "audio.wav",
            Model = "whisper"
        };

        var client = new AudioClient(CreateMockHttpClient(response));
        var result = await client.CreateTranscriptionAsync(request);

        Assert.Equal("Hello", result.Response!.Text);
    }

    [Fact]
    public async Task VideoClient_CreateAsync_ReturnsId()
    {
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("""{"id":"video_1"}""")
        };

        var client = new VideoClient(CreateMockHttpClient(response));
        var result = await client.CreateAsync(new CreateVideoRequest { Model = "video-model" });

        Assert.Equal("video_1", result.Id);
    }
}
