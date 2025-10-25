using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Together.Models.Audio;

public class AudioSpeechRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("input")]
    public string Input { get; set; } = string.Empty;

    [JsonPropertyName("voice")]
    public string? Voice { get; set; }

    [JsonPropertyName("response_format")]
    public string ResponseFormat { get; set; } = "wav";

    [JsonPropertyName("language")]
    public string Language { get; set; } = "en";

    [JsonPropertyName("response_encoding")]
    public string ResponseEncoding { get; set; } = "pcm_f32le";

    [JsonPropertyName("sample_rate")]
    public int SampleRate { get; set; } = 44100;

    [JsonPropertyName("stream")]
    public bool Stream { get; set; }
}

public class AudioSpeechStreamChunk
{
    [JsonPropertyName("b64")]
    public string Base64 { get; set; } = string.Empty;
}

public class AudioSpeechResult
{
    public byte[]? Data { get; init; }

    public IAsyncEnumerable<byte[]>? Stream { get; init; }
}

public class AudioFileRequest
{
    public Stream Content { get; set; } = Stream.Null;
    public string FileName { get; set; } = "audio.wav";
    public string Model { get; set; } = "openai/whisper-large-v3";
    public string ResponseFormat { get; set; } = "json";
    public string? Language { get; set; }
    public string? Prompt { get; set; }
    public double Temperature { get; set; } = 0.0;
    public IEnumerable<string>? TimestampGranularities { get; set; }
    public Dictionary<string, string>? AdditionalFields { get; set; }
}

public class AudioTranscriptionResponse
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}

public class AudioTranscriptionResult
{
    public AudioTranscriptionResponse? Response { get; set; }
    public AudioTranscriptionVerboseResponse? VerboseResponse { get; set; }
    public string? RawJson { get; set; }
}

public class AudioTranscriptionVerboseResponse
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("language")]
    public string? Language { get; set; }

    [JsonPropertyName("duration")]
    public double? Duration { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("segments")]
    public JsonElement? Segments { get; set; }

    [JsonPropertyName("words")]
    public JsonElement? Words { get; set; }

    [JsonPropertyName("speaker_segments")]
    public JsonElement? SpeakerSegments { get; set; }
}

public class AudioTranslationResponse
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}

public class AudioTranslationResult
{
    public AudioTranslationResponse? Response { get; set; }
    public AudioTranslationVerboseResponse? VerboseResponse { get; set; }
    public string? RawJson { get; set; }
}

public class AudioTranslationVerboseResponse
{
    [JsonPropertyName("task")]
    public string? Task { get; set; }

    [JsonPropertyName("language")]
    public string? Language { get; set; }

    [JsonPropertyName("duration")]
    public double? Duration { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("segments")]
    public JsonElement? Segments { get; set; }

    [JsonPropertyName("words")]
    public JsonElement? Words { get; set; }
}
