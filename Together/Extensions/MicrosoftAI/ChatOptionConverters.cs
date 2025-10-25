using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.AI;
using Together.Models.ChatCompletions;

namespace Together.Extensions.MicrosoftAI;

internal static class ChatOptionConverters
{
    public static ChatCompletionRequest ToChatCompletionRequest(this IEnumerable<ChatMessage> messages, ChatOptions? options, string defaultModel)
    {
        var model = options?.ModelId ?? defaultModel;
        if (string.IsNullOrWhiteSpace(model))
        {
            throw new InvalidOperationException("A model identifier must be provided through ChatOptions.ModelId or constructor.");
        }

        var request = new ChatCompletionRequest
        {
            Model = model,
            Messages = messages.Select(ConvertMessage).ToList(),
            Temperature = options?.Temperature,
            TopP = options?.TopP,
            TopK = options?.TopK,
            PresencePenalty = options?.PresencePenalty,
            FrequencyPenalty = options?.FrequencyPenalty,
            MaxTokens = options?.MaxOutputTokens,
            Seed = options?.Seed is long seedValue ? (ulong?)seedValue : null,
            Stop = options?.StopSequences?.ToList(),
            Tools = options?.Tools?.Select(ConvertTool).ToList()
        };

        if (options?.ToolMode is RequiredChatToolMode requiredMode)
        {
            request.ToolChoice = string.IsNullOrEmpty(requiredMode.RequiredFunctionName)
                ? "required"
                : new ToolChoice
                {
                    Type = "function",
                    Function = new FunctionToolChoice { Name = requiredMode.RequiredFunctionName }
                };
        }
        else if (options?.ToolMode is AutoChatToolMode)
        {
            request.ToolChoice = "auto";
        }
        else if (options?.ToolMode is NoneChatToolMode)
        {
            request.ToolChoice = "none";
        }

        if (options?.AdditionalProperties is { Count: > 0 })
        {
            request.AdditionalParameters = new Dictionary<string, object?>();
            foreach (var property in options.AdditionalProperties)
            {
                request.AdditionalParameters[property.Key] = property.Value;
            }
        }

        return request;
    }

    private static ChatCompletionMessage ConvertMessage(ChatMessage message)
    {
        var text = message.Text;
        if (string.IsNullOrWhiteSpace(text) && message.Contents is { Count: > 0 })
        {
            text = string.Join("\n", message.Contents.OfType<TextContent>().Select(c => c.Text));
        }

        return new ChatCompletionMessage
        {
            Role = message.Role,
            Content = text,
            ToolCalls = message.Contents?.OfType<FunctionCallContent>()
                .Select(call => new ToolCall
                {
                    Id = string.IsNullOrEmpty(call.CallId) ? Guid.NewGuid().ToString() : call.CallId,
                    Type = "function",
                    Function = new FunctionCall
                    {
                        Name = call.Name,
                        Arguments = call.Arguments is { Count: > 0 } args
                            ? JsonSerializer.Serialize(args)
                            : "{}"
                    }
                })
                .ToList()
        };
    }

    private static Tool ConvertTool(AITool tool)
    {
        return new Tool
        {
            Type = "function",
            Function = new FunctionTool
            {
                Name = tool.Name,
                Description = tool.Description,
                Parameters = tool.AdditionalProperties is { Count: > 0 }
                    ? tool.AdditionalProperties.ToDictionary(static kvp => kvp.Key, static kvp => kvp.Value)
                    : new Dictionary<string, object?>()
            }
        };
    }
}
