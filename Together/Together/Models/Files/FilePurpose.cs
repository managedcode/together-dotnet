using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Together.Models.Files;

/// <summary>
/// Describes the file purpose.
/// </summary>
[JsonConverter(typeof(FilePurposeConverter))]
public readonly struct FilePurpose : IEquatable<FilePurpose>
{
    public static FilePurpose FineTune { get; } = new("fine-tune");

    public string Value { get; }

    [JsonConstructor]
    public FilePurpose(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    public static bool operator ==(FilePurpose left, FilePurpose right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FilePurpose left, FilePurpose right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is FilePurpose other && Equals(other);

    public bool Equals(FilePurpose other)
        => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode()
        => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    public override string ToString() => Value;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class FilePurposeConverter : JsonConverter<FilePurpose>
    {
        public override FilePurpose Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, FilePurpose value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.Value);
    }
}