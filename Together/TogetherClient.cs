using Together.Clients;

namespace Together;

public class TogetherClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.together.xyz/";

    public TogetherClient(HttpClient httpClient, string apiKey, string baseUrl = null)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl ?? BaseUrl)
        };
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        
        Completions = new CompletionClient(_httpClient);
        ChatCompletions = new ChatCompletionClient(_httpClient);
        Embeddings = new EmbeddingClient(_httpClient);
        Files = new FileClient(_httpClient);
        FineTune = new FineTuneClient(_httpClient);
        Images = new ImageClient(_httpClient);
        Models = new ModelClient(_httpClient);
        Rerank = new RerankClient(_httpClient);
    }

    public CompletionClient Completions { get; }
    public ChatCompletionClient ChatCompletions { get; }
    public EmbeddingClient Embeddings { get; }
    public FileClient Files { get; }
    public FineTuneClient FineTune { get; }
    public ImageClient Images { get; }
    public ModelClient Models { get; }
    public RerankClient Rerank { get; }
}