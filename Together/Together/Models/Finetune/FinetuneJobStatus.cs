using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Together.Models.Finetune;

/// <summary>
///     Describes the fine-tune job status.
/// </summary>
[JsonConverter(typeof(FinetuneJobStatusConverter))]
public readonly struct FinetuneJobStatus : IEquatable<FinetuneJobStatus>
{
    public static FinetuneJobStatus Pending { get; } = new("pending");
    public static FinetuneJobStatus Queued { get; } = new("queued");
    public static FinetuneJobStatus Running { get; } = new("running");
    public static FinetuneJobStatus Compressing { get; } = new("compressing");
    public static FinetuneJobStatus Uploading { get; } = new("uploading");
    public static FinetuneJobStatus CancelRequested { get; } = new("cancel_requested");
    public static FinetuneJobStatus Cancelled { get; } = new("cancelled");
    public static FinetuneJobStatus Error { get; } = new("error");
    public static FinetuneJobStatus UserError { get; } = new("user_error");
    public static FinetuneJobStatus Completed { get; } = new("completed");

    public string Value { get; }

    [JsonConstructor]
    public FinetuneJobStatus(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    public static bool operator ==(FinetuneJobStatus left, FinetuneJobStatus right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FinetuneJobStatus left, FinetuneJobStatus right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is FinetuneJobStatus other && Equals(other);
    }

    public bool Equals(FinetuneJobStatus other)
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
    public sealed class FinetuneJobStatusConverter : JsonConverter<FinetuneJobStatus>
    {
        public override FinetuneJobStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new FinetuneJobStatus(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, FinetuneJobStatus value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}