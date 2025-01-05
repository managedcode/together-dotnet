using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Together.Models.Models;

/// <summary>
///     Describes the model type.
/// </summary>
[JsonConverter(typeof(ModelTypeConverter))]
public readonly struct ModelType : IEquatable<ModelType>
{
    public static ModelType Chat { get; } = new("chat");
    public static ModelType Language { get; } = new("language");
    public static ModelType Code { get; } = new("code");
    public static ModelType Image { get; } = new("image");
    public static ModelType Embedding { get; } = new("embedding");
    public static ModelType Moderation { get; } = new("moderation");
    public static ModelType Rerank { get; } = new("rerank");

    public string Value { get; }

    [JsonConstructor]
    public ModelType(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    public static bool operator ==(ModelType left, ModelType right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ModelType left, ModelType right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is ModelType other && Equals(other);
    }

    public bool Equals(ModelType other)
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
    public sealed class ModelTypeConverter : JsonConverter<ModelType>
    {
        public override ModelType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new ModelType(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, ModelType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}