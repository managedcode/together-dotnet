using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Together.Models.Audio;

namespace Together.Clients;

public class AudioClient(HttpClient httpClient) : BaseClient(httpClient)
{
    public async Task<AudioSpeechResult> CreateSpeechAsync(AudioSpeechRequest request, CancellationToken cancellationToken = default)
    {
        var response = await SendRequestAsync<AudioSpeechRequest, HttpResponseMessage>("/audio/speech", request, cancellationToken);
        if (request.Stream)
        {
            return new AudioSpeechResult
            {
                Stream = ParseSpeechStreamAsync(response, cancellationToken)
            };
        }

        var data = await response.Content.ReadAsByteArrayAsync(cancellationToken);
        return new AudioSpeechResult { Data = data };
    }

    private async IAsyncEnumerable<byte[]> ParseSpeechStreamAsync(HttpResponseMessage response, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream, Encoding.UTF8);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync(cancellationToken);
            if (line is null)
            {
                continue;
            }

            if (!line.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var payload = line.Substring("data:".Length).Trim();
            if (string.Equals(payload, "[DONE]", StringComparison.Ordinal))
            {
                yield break;
            }

            if (string.IsNullOrWhiteSpace(payload))
            {
                continue;
            }

            var chunk = JsonSerializer.Deserialize<AudioSpeechStreamChunk>(payload);
            if (chunk?.Base64 is { Length: > 0 } b64 && TryDecodeBase64(b64, out var data))
            {
                yield return data;
            }
        }
    }

    public async Task<AudioTranscriptionResult> CreateTranscriptionAsync(AudioFileRequest request, CancellationToken cancellationToken = default)
    {
        using var content = BuildMultipartContent(request);
        using var response = await SendRequestAsync<HttpResponseMessage>("/audio/transcriptions", HttpMethod.Post, content, cancellationToken);
        var payload = await response.Content.ReadAsStringAsync(cancellationToken);
        return BuildTranscriptionResult(request.ResponseFormat, payload);
    }

    public async Task<AudioTranslationResult> CreateTranslationAsync(AudioFileRequest request, CancellationToken cancellationToken = default)
    {
        using var content = BuildMultipartContent(request);
        using var response = await SendRequestAsync<HttpResponseMessage>("/audio/translations", HttpMethod.Post, content, cancellationToken);
        var payload = await response.Content.ReadAsStringAsync(cancellationToken);
        return BuildTranslationResult(request.ResponseFormat, payload);
    }

    private static MultipartFormDataContent BuildMultipartContent(AudioFileRequest request)
    {
        var content = new MultipartFormDataContent();

        var streamContent = new StreamContent(request.Content);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(streamContent, "file", request.FileName);

        content.Add(new StringContent(request.Model), "model");
        content.Add(new StringContent(request.ResponseFormat), "response_format");
        content.Add(new StringContent(request.Temperature.ToString(System.Globalization.CultureInfo.InvariantCulture)), "temperature");

        if (!string.IsNullOrWhiteSpace(request.Language))
        {
            content.Add(new StringContent(request.Language), "language");
        }

        if (!string.IsNullOrWhiteSpace(request.Prompt))
        {
            content.Add(new StringContent(request.Prompt), "prompt");
        }

        if (request.TimestampGranularities is not null)
        {
            foreach (var granularity in request.TimestampGranularities)
            {
                content.Add(new StringContent(granularity), "timestamp_granularities");
            }
        }

        if (request.AdditionalFields is not null)
        {
            foreach (var (key, value) in request.AdditionalFields)
            {
                content.Add(new StringContent(value), key);
            }
        }

        return content;
    }

    private static AudioTranscriptionResult BuildTranscriptionResult(string responseFormat, string json)
    {
        var result = new AudioTranscriptionResult { RawJson = json };
        var format = responseFormat?.ToLowerInvariant();
        result.VerboseResponse = format == "verbose_json"
            ? JsonSerializer.Deserialize<AudioTranscriptionVerboseResponse>(json)
            : null;
        result.Response = format == "verbose_json"
            ? null
            : JsonSerializer.Deserialize<AudioTranscriptionResponse>(json);

        return result;
    }

    private static AudioTranslationResult BuildTranslationResult(string responseFormat, string json)
    {
        var result = new AudioTranslationResult { RawJson = json };
        var format = responseFormat?.ToLowerInvariant();
        result.VerboseResponse = format == "verbose_json"
            ? JsonSerializer.Deserialize<AudioTranslationVerboseResponse>(json)
            : null;
        result.Response = format == "verbose_json"
            ? null
            : JsonSerializer.Deserialize<AudioTranslationResponse>(json);

        return result;
    }

    private static bool TryDecodeBase64(string value, out byte[] data)
    {
        try
        {
            data = Convert.FromBase64String(value);
            return true;
        }
        catch (FormatException)
        {
            data = Array.Empty<byte>();
            return false;
        }
    }
}
