using Microsoft.Extensions.AI;
using Together.Clients;

namespace Together.Extensions.MicrosoftAI;

public static class TogetherAIClientExtensions
{
    public static IChatClient AsMicrosoftAIChatClient(this TogetherClient client, string defaultModel)
    {
        return new TogetherAIChatClient(client.ChatCompletions, defaultModel);
    }

    public static IChatClient AsMicrosoftAIImageClient(this TogetherClient client, string defaultModel)
    {
        return new TogetherAIImageClient(client.Images, defaultModel);
    }

    public static IEmbeddingGenerator<string, Embedding<float>> AsMicrosoftAIEmbeddingGenerator(this TogetherClient client, string defaultModel)
    {
        return new TogetherAIEmbeddingGenerator(client.Embeddings, defaultModel);
    }

    public static ISpeechToTextClient AsMicrosoftAISpeechToTextClient(this TogetherClient client, string defaultModel)
    {
        return new TogetherAISpeechToTextClient(client.Audio, defaultModel);
    }
}
