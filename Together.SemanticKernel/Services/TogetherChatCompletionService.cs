﻿using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.TextGeneration;
using Together.Models.ChatCompletions;
using Together.Models.Common;
using Together.SemanticKernel.Extensions;
using TextContent = Microsoft.SemanticKernel.TextContent;

namespace Together.SemanticKernel.Services;

public sealed class TogetherChatCompletionService : IChatCompletionService, ITextGenerationService
{
    private const int MaxInflightAutoInvokes = 128;
    private static readonly Meter s_meter = new("Microsoft.SemanticKernel.Connectors.Together");
    private static readonly Counter<int> s_promptTokensCounter = s_meter.CreateCounter<int>("semantic_kernel.connectors.together.tokens.prompt");

    private static readonly Counter<int> s_completionTokensCounter =
        s_meter.CreateCounter<int>("semantic_kernel.connectors.together.tokens.completion");

    private static readonly Counter<int> s_totalTokensCounter = s_meter.CreateCounter<int>("semantic_kernel.connectors.together.tokens.total");
    private readonly Dictionary<string, object?> _attributes = new();

    private readonly TogetherClient _client;
    private readonly ILogger<TogetherChatCompletionService> _logger;
    private readonly string _model;

    public TogetherChatCompletionService(TogetherClient togetherClient, string model, ILogger<TogetherChatCompletionService>? logger)
    {
        ArgumentNullException.ThrowIfNull(togetherClient);
        ArgumentException.ThrowIfNullOrWhiteSpace(model);

        _client = togetherClient;
        _model = model;
        _logger = logger ?? NullLogger<TogetherChatCompletionService>.Instance;
        _attributes.Add("ModelId", model);
    }

    public IReadOnlyDictionary<string, object?> Attributes => _attributes;

    public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
        try
        {
            using var activity = StartActivity("ChatCompletion");

            for (var requestIndex = 0;; requestIndex++)
            {
                var request = new ChatCompletionRequest
                {
                    Model = _model,
                    Messages = chatHistory.ToChatCompletionMessages()
                        .ToList(),
                    Stream = false
                };

                var toolConfig = GetToolConfiguration(kernel, chatHistory, executionSettings, requestIndex);
                if (toolConfig.HasTools)
                {
                    ConfigureTools(kernel, request);
                }

                var response = await _client.ChatCompletions.CreateAsync(request, cancellationToken);
                LogUsage(new UsageData
                {
                    PromptTokens = response.Usage.PromptTokens,
                    CompletionTokens = response.Usage.CompletionTokens,
                    TotalTokens = response.Usage.TotalTokens
                });

                if (response.Choices?.FirstOrDefault()
                        ?.Message == null)
                {
                    return Array.Empty<ChatMessageContent>();
                }

                var result = response.Choices.First();
                var message = new ChatCompletionMessage
                {
                    Role = result.Message.Role,
                    Content = result.Message.Content,
                    ToolCalls = result.Message
                        .ToolCalls
                        ?.Select(t => new ToolCall
                        {
                            Id = t.Id,
                            Type = t.Type,
                            Function = new FunctionCall
                            {
                                Name = t.Function.Name,
                                Arguments = t.Function.Arguments
                            }
                        })
                        .ToList()
                };
                
                var messageContent = CreateChatMessageContent(message);
                
                // If no tool calls or no auto-invoke, return response
                if (!toolConfig.AutoInvoke || result.Message.ToolCalls?.Any() != true)
                {
                    return [messageContent];
                }
                
                //add history
                chatHistory.Add(messageContent);

                // Process tool calls asynchronously
                foreach (var toolCall in result.Message.ToolCalls)
                {
                    var process = await ProcessToolCallAsync(new ToolCall
                    {
                        Id = toolCall.Id,
                        Type = toolCall.Type,
                        Function = new FunctionCall
                        {
                            Name = toolCall.Function.Name,
                            Arguments = toolCall.Function.Arguments
                        }
                    }, kernel, chatHistory, cancellationToken);
                    
                    if (!process)
                    {
                        _logger.LogWarning("Failed to process tool call: {ToolCall}", toolCall.Function?.Name);
                    }
                }

                // Get final response with function results
                request.Messages = chatHistory.ToChatCompletionMessages()
                    .ToList();
                var finalResponse = await _client.ChatCompletions.CreateAsync(request, cancellationToken);

                if (finalResponse?.Choices?.FirstOrDefault()
                        ?.Message != null)
                {
                    return new[]
                    {
                        CreateChatMessageContent(finalResponse.Choices.First()
                            .Message)
                    };
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in chat completion");
            throw new KernelException("Chat completion failed", ex);
        }
    }

    public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null, Kernel? kernel = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new ChatCompletionRequest
        {
            Model = _model,
            Messages = chatHistory.ToChatCompletionMessages()
                .ToList(),
            Stream = true
        };
        if (kernel != null && executionSettings?.ExtensionData?.ContainsKey("tool_choice") == true)
        {
            ConfigureTools(kernel, request);
        }

        var stream = _client.ChatCompletions.CreateStreamAsync(request, cancellationToken);

        await foreach (var chunk in stream)
        {
            foreach (var choice in chunk.Choices)
            {
                yield return new StreamingChatMessageContent(AuthorRole.Assistant, choice.Delta?.Content, null, choice.Index.GetValueOrDefault(),
                    chunk.Model);
            }
        }
    }

    public Task<IReadOnlyList<TextContent>> GetTextContentsAsync(string prompt, PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
        // Convert text prompt to chat format
        var chatHistory = new ChatHistory();
        chatHistory.AddUserMessage(prompt);
        return GetChatMessageContentsAsync(chatHistory, executionSettings, kernel, cancellationToken)
            .ContinueWith(t => (IReadOnlyList<TextContent>)t.Result
                .Select(m => new TextContent(m.Content))
                .ToList(), cancellationToken);
    }

    public async IAsyncEnumerable<StreamingTextContent> GetStreamingTextContentsAsync(string prompt,
        PromptExecutionSettings? executionSettings = null, Kernel? kernel = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var chatHistory = new ChatHistory();
        chatHistory.AddUserMessage(prompt);

        await foreach (var message in GetStreamingChatMessageContentsAsync(chatHistory, executionSettings, kernel, cancellationToken))
        {
            yield return new StreamingTextContent(message.Content);
        }
    }

    // Modified to async: renamed to ProcessToolCallAsync
    private async Task<bool> ProcessToolCallAsync(ToolCall toolCall, Kernel? kernel, ChatHistory chatHistory, CancellationToken cancellationToken)
    {
        if (kernel == null || string.IsNullOrEmpty(toolCall.Function?.Name))
        {
            return false;
        }

        try
        {
            var functionName = ParseFunctionName(toolCall.Function.Name);
            if (!kernel.Plugins.TryGetFunction(functionName.PluginName, functionName.Name, out var function))
            {
                _logger.LogWarning("Function not found: {Function}", toolCall.Function.Name);
                return false;
            }

            var args = ParseArguments(toolCall.Function.Arguments);
            // Await asynchronous function invocation instead of using .Result
            var result = await function.InvokeAsync(kernel, args, cancellationToken);

            chatHistory.Add(new ChatMessageContent(AuthorRole.Tool, result.ToString(),
                metadata: new Dictionary<string, object?> { { "tool_call_id", toolCall.Id } }));

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing tool call {Name}", toolCall.Function.Name);
            return false;
        }
    }

    private KernelArguments ParseArguments(string argumentJson)
    {
        try
        {
            var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(argumentJson);
            return new KernelArguments(dict ?? new Dictionary<string, object>());
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse function arguments: {Arguments}", argumentJson);
            return new KernelArguments();
        }
    }

    private ToolConfiguration GetToolConfiguration(Kernel? kernel, ChatHistory chatHistory, PromptExecutionSettings? settings, int requestIndex)
    {
        if (kernel == null)
        {
            return new ToolConfiguration(false, false, 0);
        }

        // Check if kernel has any plugins/functions available
        var hasTools = kernel.Plugins
            .GetFunctionsMetadata()
            .Any();
        if (!hasTools)
        {
            return new ToolConfiguration(false, false, 0);
        }
        
        if(settings is TogetherPromptExecutionSettings togetherSettings)
        {
            var call = togetherSettings.FunctionChoiceBehavior?.GetConfiguration(new FunctionChoiceBehaviorConfigurationContext(chatHistory)
            {
                Kernel = kernel
            });
            return new ToolConfiguration(hasTools, call?.AutoInvoke ?? false, 1);
        }
        
        //TODO: check type of settings

        // Check execution settings
        var autoInvoke = false;
        var maxAttempts = 1;

        if (settings?.ExtensionData != null)
        {
            autoInvoke = settings.ExtensionData.TryGetValue("auto_invoke", out var autoInvokeObj) && autoInvokeObj is bool autoInvokeValue &&
                         autoInvokeValue;

            maxAttempts = settings.ExtensionData.TryGetValue("max_attempts", out var maxAttemptsObj) && maxAttemptsObj is int maxAttemptsValue
                ? maxAttemptsValue
                : 1;
        }

        // Disable auto-invoke if we've exceeded attempts limit
        if (requestIndex >= maxAttempts || requestIndex >= MaxInflightAutoInvokes)
        {
            autoInvoke = false;
        }

        return new ToolConfiguration(hasTools, autoInvoke, maxAttempts);
    }

    private FunctionName ParseFunctionName(string fullName)
    {
        var parts = fullName.Split(':');
        return parts.Length > 1 ? new FunctionName(parts[0], parts[1]) : new FunctionName(string.Empty, parts[0]);
    }

    private Activity? StartActivity(string name)
    {
        return Activity.Current?.Source.StartActivity(name);
    }

    private void LogUsage(UsageData? usage)
    {
        if (usage == null)
        {
            return;
        }

        s_promptTokensCounter.Add(usage.PromptTokens);
        s_completionTokensCounter.Add(usage.CompletionTokens);
        s_totalTokensCounter.Add(usage.TotalTokens);

        _logger.LogInformation("Tokens - Prompt: {PromptTokens}, Completion: {CompletionTokens}, Total: {TotalTokens}", usage.PromptTokens,
            usage.CompletionTokens, usage.TotalTokens);
    }

    private ChatMessageContent CreateChatMessageContent(ChatCompletionMessage message)
    {
        return new ChatMessageContent(AuthorRole.Assistant, message.Content, metadata: new Dictionary<string, object?>
        {
            { "tool_calls", message.ToolCalls }
        });
    }

    private void ConfigureTools(Kernel kernel, ChatCompletionRequest request)
    {
        var functions = kernel.Plugins.GetFunctionsMetadata();
        if (!functions.Any())
        {
            return;
        }

        request.Tools = functions.Select(f => new Tool
            {
                Type = "function",
                Function = new FunctionTool
                {
                    Name = $"{f.PluginName}:{f.Name}",
                    Description = f.Description,
                    Parameters = new Dictionary<string, object>
                    {
                        { "type", "object" },
                        {
                            "properties",
                            f.Parameters.ToDictionary(p => p.Name, p => new { type = p.ParameterType?.Name.ToLower(), description = p.Description })
                        },
                        {
                            "required", f.Parameters
                                .Where(p => p.IsRequired)
                                .Select(p => p.Name)
                                .ToList()
                        }
                    }
                }
            })
            .ToList();

        
        request.ToolChoice = new ToolChoice
        {
            Type = "auto",
            Function = new FunctionToolChoice { Name = "auto" },
        };
    }

    private async Task<ChatCompletionResponse> GetCompletionAsync(IEnumerable<ChatCompletionMessage> messages,
        PromptExecutionSettings? settings = null, CancellationToken cancellationToken = default)
    {
        // var options = new Dictionary<string, object>();
        //
        // if (settings?.ExtensionData != null)
        // {
        //     foreach (var kvp in settings.ExtensionData)
        //     {
        //         options[kvp.Key] = options[kvp.Value];
        //     }
        // }

        var request = new ChatCompletionRequest
        {
            Messages = messages.ToList(),
            Model = _model,
            Stream = false
            //Options = options
        };

        return await _client.ChatCompletions.CreateAsync(request, cancellationToken);
    }

    private record ToolConfiguration(bool HasTools, bool AutoInvoke, int MaxAttempts);

    private record FunctionName(string PluginName, string Name);
}