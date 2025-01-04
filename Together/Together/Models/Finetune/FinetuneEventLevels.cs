using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Together.Models.Finetune;

/// <summary>
/// Describes the fine-tune job event status levels.
/// </summary>
[JsonConverter(typeof(FinetuneEventLevelsConverter))]
public readonly struct FinetuneEventLevels : IEquatable<FinetuneEventLevels>
{
    public static FinetuneEventLevels Null { get; } = new("");
    public static FinetuneEventLevels Info { get; } = new("Info");
    public static FinetuneEventLevels Warning { get; } = new("Warning");
    public static FinetuneEventLevels Error { get; } = new("Error");
    public static FinetuneEventLevels LegacyInfo { get; } = new("info");
    public static FinetuneEventLevels LegacyWarning { get; } = new("warning");
    public static FinetuneEventLevels LegacyError { get; } = new("error");

    public string Value { get; }

    [JsonConstructor]
    public FinetuneEventLevels(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    public static bool operator ==(FinetuneEventLevels left, FinetuneEventLevels right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FinetuneEventLevels left, FinetuneEventLevels right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is FinetuneEventLevels other && Equals(other);

    public bool Equals(FinetuneEventLevels other)
        => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode()
        => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    public override string ToString() => Value;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class FinetuneEventLevelsConverter : JsonConverter<FinetuneEventLevels>
    {
        public override FinetuneEventLevels Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, FinetuneEventLevels value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.Value);
    }
}