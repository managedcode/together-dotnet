using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Together.Models.Finetune;

/// <summary>
///     Describes the download checkpoint type.
/// </summary>
[JsonConverter(typeof(DownloadCheckpointTypeConverter))]
public readonly struct DownloadCheckpointType : IEquatable<DownloadCheckpointType>
{
    public static DownloadCheckpointType Default { get; } = new("default");
    public static DownloadCheckpointType Merged { get; } = new("merged");
    public static DownloadCheckpointType Adapter { get; } = new("adapter");

    public string Value { get; }

    [JsonConstructor]
    public DownloadCheckpointType(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    public static bool operator ==(DownloadCheckpointType left, DownloadCheckpointType right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(DownloadCheckpointType left, DownloadCheckpointType right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is DownloadCheckpointType other && Equals(other);
    }

    public bool Equals(DownloadCheckpointType other)
    {
        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    }

    public override string ToString()
    {
        return Value;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class DownloadCheckpointTypeConverter : JsonConverter<DownloadCheckpointType>
    {
        public override DownloadCheckpointType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new DownloadCheckpointType(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, DownloadCheckpointType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}