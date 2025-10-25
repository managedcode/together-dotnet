# Together API Coverage

The table below tracks parity between the Together .NET SDK and the reference SDKs in [together-python](https://github.com/togethercomputer/together-python) and [together-typescript](https://github.com/togethercomputer/together-typescript).

| Capability | REST endpoint(s) | .NET Support | Notes |
| --- | --- | --- | --- |
| Text completions | `/completions` | `TogetherClient.Completions` | Streaming and non-streaming requests |
| Chat completions & tool calling | `/chat/completions` | `TogetherClient.ChatCompletions` | Function/tool calling supported |
| Embeddings | `/embeddings` | `TogetherClient.Embeddings` | Includes Microsoft.Extensions.AI adapter |
| Rerank | `/rerank` | `TogetherClient.Rerank` | Matches Python/TypeScript clients |
| Files | `/files` | `TogetherClient.Files` | Upload/list/delete supported |
| Fine-tuning & checkpoints | `/fine-tunes` | `TogetherClient.FineTune` | Training limits, cancellation, download helpers |
| Model catalog | `/models` | `TogetherClient.Models` | Lists and describes models |
| Images | `/images/generations` | `TogetherClient.Images` | Added Microsoft.Extensions.AI image adapter |
| Audio speech (text-to-speech) | `/audio/speech` | `TogetherClient.Audio.CreateSpeechAsync` | Streaming + full responses |
| Audio transcription | `/audio/transcriptions` | `TogetherClient.Audio.CreateTranscriptionAsync` | Multipart uploads, verbose JSON |
| Audio translation | `/audio/translations` | `TogetherClient.Audio.CreateTranslationAsync` | Mirrors Python behaviour |
| Evaluation jobs | `/evaluation` & `/evaluations` | `TogetherClient.Evaluation` | Create, list, status, model list |
| Batches | `/batches` | `TogetherClient.Batches` | Create/list/retrieve/cancel |
| Endpoints | `/endpoints` | `TogetherClient.Endpoints` | Create/list/delete dedicated endpoints |
| Hardware catalog | `/hardware` | `TogetherClient.Hardware` | Availability and pricing data |
| Jobs status | `/jobs` | `TogetherClient.Jobs` | List/retrieve background jobs |
| Code interpreter | `/tci/execute` | `TogetherClient.CodeInterpreter` | Handles file uploads and execution |
| Video generation | `/v2/videos` | `TogetherClient.Videos` | Create & retrieve video jobs |
| Microsoft.Extensions.AI (text) | Chat completions | `TogetherAIChatClient` | Implements `IChatClient` |
| Microsoft.Extensions.AI (embeddings) | Embeddings | `TogetherAIEmbeddingGenerator` | Implements `IEmbeddingGenerator<string, Embedding<float>>` |
| Microsoft.Extensions.AI (speech-to-text) | Audio transcription | `TogetherAISpeechToTextClient` | Implements `ISpeechToTextClient` |
| Microsoft.Extensions.AI (image generation) | Image generation | `TogetherAIImageClient` | Implements `IChatClient` producing image content |

Each adapter has unit tests in `Together.Tests/MicrosoftAI` to guard behaviour.
