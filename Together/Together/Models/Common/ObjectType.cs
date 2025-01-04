using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Shared.Diagnostics;

namespace Together.Models.Common;

/// <summary>
/// Describes the type of an object within the system.
/// </summary>
[JsonConverter(typeof(ObjectTypeConverter))]
public readonly struct ObjectType : IEquatable<ObjectType>
{
    public static ObjectType Completion { get; } = new("text.completion");
    public static ObjectType CompletionChunk { get; } = new("completion.chunk");
    public static ObjectType ChatCompletion { get; } = new("chat.completion");
    public static ObjectType ChatCompletionChunk { get; } = new("chat.completion.chunk");
    public static ObjectType Embedding { get; } = new("embedding");
    public static ObjectType FinetuneEvent { get; } = new("fine-tune-event");
    public static ObjectType File { get; } = new("file");
    public static ObjectType Model { get; } = new("model");

    public string Value { get; }

    [JsonConstructor]
    public ObjectType(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    public static bool operator ==(ObjectType left, ObjectType right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ObjectType left, ObjectType right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is ObjectType other && Equals(other);

    public bool Equals(ObjectType other)
        => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode()
        => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    public override string ToString() => Value;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class ObjectTypeConverter : JsonConverter<ObjectType>
    {
        public override ObjectType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ObjectType value, JsonSerializerOptions options) => 
            writer.WriteStringValue(value.Value);

    }
}