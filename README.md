# Together .NET SDK

C# SDK for Together.ai

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
  - [Initialization](#initialization)
  - [Completions](#completions)
  - [Chat Completions](#chat-completions)
  - [Embeddings](#embeddings)
  - [Images](#images)
- [Constants](#constants)
- [Contributing](#contributing)
- [License](#license)

## Introduction

The Together .NET SDK provides a simple and efficient way to interact with the Together.ai API using C#. This SDK allows you to easily integrate various AI capabilities such as completions, chat completions, embeddings, and image generations into your .NET applications.

## Features

- **Completions**: Generate text completions based on a given prompt.
- **Chat Completions**: Generate chat-based completions for conversational AI.
- **Embeddings**: Generate vector embeddings for text.
- **Images**: Generate images based on a given prompt.

## Installation

To install the Together .NET SDK, add the following package to your project:

```sh
dotnet add package Together
```

## Usage

### Initialization

To use the SDK, you need to initialize the `TogetherClient` with an `HttpClient`:

```csharp
using Together;
using System.Net.Http.Headers;

var httpClient = new HttpClient { BaseAddress = new Uri(TogetherConstants.BASE_URL) };
httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YOUR_API_KEY");
var client = new TogetherClient(httpClient);
```

### Completions

To get a text completion:

```csharp
var request = new CompletionRequest { Prompt = "Hello, world!" };
var response = await client.GetCompletionResponseAsync(request);
Console.WriteLine(response.Choices.First().Text);
```

### Chat Completions

To get a chat completion:

```csharp
var request = new ChatCompletionRequest
{
    Messages = new List<ChatCompletionMessage> { new ChatCompletionMessage { Role = "user", Content = "Hello!" } }
};
var response = await client.GetChatCompletionResponseAsync(request);
Console.WriteLine(response.Choices.First().Message.Content);
```

### Embeddings

To get embeddings:

```csharp
var request = new EmbeddingRequest { Input = "Hello, world!" };
var response = await client.GetEmbeddingResponseAsync(request);
Console.WriteLine(string.Join(", ", response.Data.First().Embedding));
```

### Images

To generate an image:

```csharp
var request = new ImageRequest
{
    Prompt = "A beautiful sunset over the mountains",
    Model = "black-forest-labs/FLUX.1-dev",
    N = 1,
    Steps = 10,
    Height = 512,
    Width = 512
};
var response = await client.GetImageResponseAsync(request);
Console.WriteLine(response.Data.First().Url);
```

## Constants

The SDK provides various constants that can be used throughout your application. These constants are defined in the `TogetherConstants` class.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request on GitHub.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.