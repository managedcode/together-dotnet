using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Together.Models.Finetune;

/// <summary>
///     Describes the dataset format.
/// </summary>
[JsonConverter(typeof(DatasetFormatConverter))]
public readonly struct DatasetFormat : IEquatable<DatasetFormat>
{
    public static DatasetFormat General { get; } = new("general");
    public static DatasetFormat Conversation { get; } = new("conversation");
    public static DatasetFormat Instruction { get; } = new("instruction");

    public string Value { get; }

    [JsonConstructor]
    public DatasetFormat(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    public static bool operator ==(DatasetFormat left, DatasetFormat right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(DatasetFormat left, DatasetFormat right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is DatasetFormat other && Equals(other);
    }

    public bool Equals(DatasetFormat other)
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
    public sealed class DatasetFormatConverter : JsonConverter<DatasetFormat>
    {
        public override DatasetFormat Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new DatasetFormat(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, DatasetFormat value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}