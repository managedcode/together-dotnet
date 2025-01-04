using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Together.Models.ChatCompletions;

/// <summary>
///     Describes the chat completion message content type.
/// </summary>
[JsonConverter(typeof(ChatCompletionMessageContentTypeConverter))]
public readonly struct ChatCompletionMessageContentType : IEquatable<ChatCompletionMessageContentType>
{
    public static ChatCompletionMessageContentType Text { get; } = new("text");
    public static ChatCompletionMessageContentType ImageUrl { get; } = new("image_url");

    public string Value { get; }

    [JsonConstructor]
    public ChatCompletionMessageContentType(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    public static bool operator ==(ChatCompletionMessageContentType left, ChatCompletionMessageContentType right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ChatCompletionMessageContentType left, ChatCompletionMessageContentType right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is ChatCompletionMessageContentType other && Equals(other);
    }

    public bool Equals(ChatCompletionMessageContentType other)
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
    public sealed class ChatCompletionMessageContentTypeConverter : JsonConverter<ChatCompletionMessageContentType>
    {
        public override ChatCompletionMessageContentType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new ChatCompletionMessageContentType(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, ChatCompletionMessageContentType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}