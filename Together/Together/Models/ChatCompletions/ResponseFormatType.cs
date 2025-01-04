using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Together.Models.ChatCompletions;

/// <summary>
/// Describes the response format type.
/// </summary>
[JsonConverter(typeof(ResponseFormatTypeConverter))]
public readonly struct ResponseFormatType : IEquatable<ResponseFormatType>
{
    public static ResponseFormatType JsonObject { get; } = new("json_object");
    public static ResponseFormatType JsonSchema { get; } = new("json_schema");

    public string Value { get; }

    [JsonConstructor]
    public ResponseFormatType(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    public static bool operator ==(ResponseFormatType left, ResponseFormatType right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ResponseFormatType left, ResponseFormatType right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is ResponseFormatType other && Equals(other);

    public bool Equals(ResponseFormatType other)
        => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode()
        => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    public override string ToString() => Value;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class ResponseFormatTypeConverter : JsonConverter<ResponseFormatType>
    {
        public override ResponseFormatType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ResponseFormatType value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.Value);
    }
}