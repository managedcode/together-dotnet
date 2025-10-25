using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Together.Clients;
using Together.Models.Audio;

namespace Together.Extensions.MicrosoftAI;

public class TogetherAISpeechToTextClient : ISpeechToTextClient
{
    private readonly AudioClient _audioClient;
    private readonly string _defaultModel;

    public TogetherAISpeechToTextClient(AudioClient audioClient, string defaultModel)
    {
        _audioClient = audioClient ?? throw new ArgumentNullException(nameof(audioClient));
        _defaultModel = defaultModel;
    }

    public async Task<SpeechToTextResponse> GetTextAsync(Stream audio, SpeechToTextOptions? options, CancellationToken cancellationToken = default)
    {
        if (audio is null)
        {
            throw new ArgumentNullException(nameof(audio));
        }

        var model = options?.ModelId ?? _defaultModel;
        if (string.IsNullOrWhiteSpace(model))
        {
            throw new InvalidOperationException("A model identifier must be provided for speech to text.");
        }

        var request = new AudioFileRequest
        {
            Content = audio,
            FileName = options?.AdditionalProperties?.TryGetValue("file_name", out var name) == true ? name?.ToString() ?? "audio.wav" : "audio.wav",
            Model = model,
            Language = options?.SpeechLanguage,
            ResponseFormat = "json"
        };

        var transcription = await _audioClient.CreateTranscriptionAsync(request, cancellationToken);
        var text = transcription.Response?.Text ?? transcription.VerboseResponse?.Text ?? string.Empty;

        return new SpeechToTextResponse(text)
        {
            ModelId = model
        };
    }

    public async IAsyncEnumerable<SpeechToTextResponseUpdate> GetStreamingTextAsync(Stream audio, SpeechToTextOptions? options, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var response = await GetTextAsync(audio, options, cancellationToken);
        yield return new SpeechToTextResponseUpdate(response.Text)
        {
            Kind = SpeechToTextResponseUpdateKind.TextUpdated,
            ModelId = response.ModelId
        };
    }

    public object? GetService(Type serviceType, object? serviceKey)
    {
        if (serviceType == typeof(AudioClient))
        {
            return _audioClient;
        }

        return null;
    }

    public void Dispose()
    {
    }
}
