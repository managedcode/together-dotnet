using Together.Clients;

namespace Together;

public class TogetherClient(HttpClient httpClient)
{
    // private readonly HttpClient _httpClient;
    // private const string BaseUrl = "https://api.together.xyz/";

    // public TogetherClient(string apiKey)
    // {
    //     _httpClient = new HttpClient
    //     {
    //         BaseAddress = new Uri(BaseUrl)
    //     };
    //     _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    // }

    public CompletionClient Completions { get; } = new(httpClient);
    public ChatCompletionClient ChatCompletions { get; } = new(httpClient);
    public EmbeddingClient Embeddings { get; } = new(httpClient);
    public FileClient Files { get; } = new(httpClient);
    public FineTuneClient FineTune { get; } = new(httpClient);
    public ImageClient Images { get; } = new(httpClient);
    public ModelClient Models { get; } = new(httpClient);
    public RerankClient Rerank { get; } = new(httpClient);
}