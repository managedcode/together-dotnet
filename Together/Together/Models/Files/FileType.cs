using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Together.Models.Files;

/// <summary>
/// Describes the file type.
/// </summary>
[JsonConverter(typeof(FileTypeConverter))]
public readonly struct FileType : IEquatable<FileType>
{
    public static FileType Jsonl { get; } = new("jsonl");
    public static FileType Parquet { get; } = new("parquet");

    public string Value { get; }

    [JsonConstructor]
    public FileType(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    public static bool operator ==(FileType left, FileType right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FileType left, FileType right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is FileType other && Equals(other);

    public bool Equals(FileType other)
        => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode()
        => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    public override string ToString() => Value;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class FileTypeConverter : JsonConverter<FileType>
    {
        public override FileType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, FileType value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.Value);
    }
}