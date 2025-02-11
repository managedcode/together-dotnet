# Together .NET SDK

[![.NET](https://github.com/managedcode/Together/actions/workflows/dotnet.yml/badge.svg)](https://github.com/managedcode/Together/actions/workflows/dotnet.yml)
[![NuGet](https://img.shields.io/nuget/v/ManagedCode.Together.svg)](https://www.nuget.org/packages/ManagedCode.Together)
[![NuGet](https://img.shields.io/nuget/v/ManagedCode.Together.SemanticKernel.svg)](https://www.nuget.org/packages/ManagedCode.Together.SemanticKernel)
[![Downloads](https://img.shields.io/nuget/dt/ManagedCode.Together.svg)](https://www.nuget.org/packages/ManagedCode.Together)
[![License](https://img.shields.io/github/license/managedcode/Together)](https://github.com/managedcode/Together/blob/main/LICENSE)

Unofficial C# SDK for [Together.ai](https://www.together.ai/) with Semantic Kernel integration.

## Introduction

Together.ai provides access to the latest open-source AI models through a simple API. This SDK offers:

- 🚀 Easy access to Together.ai's API from .NET applications
- 🧠 Seamless integration with Microsoft Semantic Kernel
- 🔧 Chat Completions, Text Generation, Embeddings and Image Generation
- 🌊 Both synchronous and streaming responses
- 🛠 Built-in function calling support

## Installation

Choose the package(s) you need:

```sh
# Core API client
dotnet add package ManagedCode.Together

# Semantic Kernel integration
dotnet add package ManagedCode.Together.SemanticKernel
```

## Usage Examples

### Direct API Usage

```csharp
using Together;

var client = new TogetherClient("YOUR_API_KEY");
var response = await client.ChatCompletions.CreateAsync(new ChatCompletionRequest 
{
    Model = "mistralai/Mistral-7B-Instruct-v0.1",
    Messages = new[] 
    { 
        new ChatCompletionMessage { Role = "user", Content = "Hello!" } 
    }
});
```

### Semantic Kernel Integration

```csharp
using Microsoft.SemanticKernel;
using Together.SemanticKernel;

// Initialize kernel with multiple Together.ai capabilities
var kernel = Kernel.CreateBuilder()
    .AddTogetherChatCompletion(
        "mistralai/Mistral-7B-Instruct-v0.1", 
        "YOUR_API_KEY"
    )
    .AddTogetherTextEmbeddingGeneration(
        "togethercomputer/m2-bert-80M-2k-retrieval",
        "YOUR_API_KEY"
    )
    .AddTogetherTextToImage(
        "stabilityai/stable-diffusion-xl-base-1.0",
        "YOUR_API_KEY"
    )
    .Build();

// Chat completion
var chatResult = await kernel.InvokePromptAsync("What is quantum computing?");

// Generate embeddings
var embeddingService = kernel.GetRequiredService<ITextEmbeddingGenerationService>();
var embeddings = await embeddingService.GenerateEmbeddingsAsync(
    ["What is quantum computing?"]
);

// Generate images
var imageService = kernel.GetRequiredService<ITextToImageService>();
var images = await imageService.GetImageContentsAsync(
    "A cat playing piano",
    new TogetherTextToImageExecutionSettings 
    {
        Height = 512,
        Width = 512
    }
);
```

## 📚 Documentation

For more information about available models and features, visit
the [Together.ai Documentation](https://docs.together.ai/)

## 💪 Contributing

Contributions are welcome! Feel free to:

- Open issues for bugs or feature requests
- Submit pull requests
- Improve documentation

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## ⭐ Support

If you find this project useful, please give it a star on GitHub!
