using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Shared.Diagnostics;

namespace Together.Models.Common;

/// <summary>
/// Describes the reason for finishing a process.
/// </summary>
[JsonConverter(typeof(FinishReasonConverter))]
public readonly struct FinishReason : IEquatable<FinishReason>
{
    public static FinishReason Length { get; } = new("length");
    public static FinishReason StopSequence { get; } = new("stop");
    public static FinishReason EOS { get; } = new("eos");
    public static FinishReason ToolCalls { get; } = new("tool_calls");
    public static FinishReason Error { get; } = new("error");

    public string Value { get; }

    [JsonConstructor]
    public FinishReason(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    public static bool operator ==(FinishReason left, FinishReason right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FinishReason left, FinishReason right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is FinishReason other && Equals(other);

    public bool Equals(FinishReason other)
        => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode()
        => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    public override string ToString() => Value;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class FinishReasonConverter : JsonConverter<FinishReason>
    {
        public override FinishReason Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, FinishReason value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.Value);
    }
}