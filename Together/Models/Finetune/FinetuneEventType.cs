using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Together.Models.Finetune;

/// <summary>
///     Describes the fine-tune job event types.
/// </summary>
[JsonConverter(typeof(FinetuneEventTypeConverter))]
public readonly struct FinetuneEventType : IEquatable<FinetuneEventType>
{
    public static FinetuneEventType JobPending { get; } = new("JOB_PENDING");
    public static FinetuneEventType JobStart { get; } = new("JOB_START");
    public static FinetuneEventType JobStopped { get; } = new("JOB_STOPPED");
    public static FinetuneEventType ModelDownloading { get; } = new("MODEL_DOWNLOADING");
    public static FinetuneEventType ModelDownloadComplete { get; } = new("MODEL_DOWNLOAD_COMPLETE");
    public static FinetuneEventType TrainingDataDownloading { get; } = new("TRAINING_DATA_DOWNLOADING");
    public static FinetuneEventType TrainingDataDownloadComplete { get; } = new("TRAINING_DATA_DOWNLOAD_COMPLETE");
    public static FinetuneEventType ValidationDataDownloading { get; } = new("VALIDATION_DATA_DOWNLOADING");
    public static FinetuneEventType ValidationDataDownloadComplete { get; } = new("VALIDATION_DATA_DOWNLOAD_COMPLETE");
    public static FinetuneEventType WandbInit { get; } = new("WANDB_INIT");
    public static FinetuneEventType TrainingStart { get; } = new("TRAINING_START");
    public static FinetuneEventType CheckpointSave { get; } = new("CHECKPOINT_SAVE");
    public static FinetuneEventType BillingLimit { get; } = new("BILLING_LIMIT");
    public static FinetuneEventType EpochComplete { get; } = new("EPOCH_COMPLETE");
    public static FinetuneEventType EvalComplete { get; } = new("EVAL_COMPLETE");
    public static FinetuneEventType TrainingComplete { get; } = new("TRAINING_COMPLETE");
    public static FinetuneEventType ModelCompressing { get; } = new("COMPRESSING_MODEL");
    public static FinetuneEventType ModelCompressionComplete { get; } = new("MODEL_COMPRESSION_COMPLETE");
    public static FinetuneEventType ModelUploading { get; } = new("MODEL_UPLOADING");
    public static FinetuneEventType ModelUploadComplete { get; } = new("MODEL_UPLOAD_COMPLETE");
    public static FinetuneEventType JobComplete { get; } = new("JOB_COMPLETE");
    public static FinetuneEventType JobError { get; } = new("JOB_ERROR");
    public static FinetuneEventType JobUserError { get; } = new("JOB_USER_ERROR");
    public static FinetuneEventType CancelRequested { get; } = new("CANCEL_REQUESTED");
    public static FinetuneEventType JobRestarted { get; } = new("JOB_RESTARTED");
    public static FinetuneEventType Refund { get; } = new("REFUND");
    public static FinetuneEventType Warning { get; } = new("WARNING");

    public string Value { get; }

    [JsonConstructor]
    public FinetuneEventType(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    public static bool operator ==(FinetuneEventType left, FinetuneEventType right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FinetuneEventType left, FinetuneEventType right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is FinetuneEventType other && Equals(other);
    }

    public bool Equals(FinetuneEventType other)
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
    public sealed class FinetuneEventTypeConverter : JsonConverter<FinetuneEventType>
    {
        public override FinetuneEventType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new FinetuneEventType(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, FinetuneEventType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}