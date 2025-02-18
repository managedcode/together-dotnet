﻿using Together.Clients;

namespace Together;

public class TogetherClient
{
    private const string BaseUrl = "https://api.together.xyz/";
    private readonly HttpClient _httpClient;

    public TogetherClient(string apiKey) : this(apiKey, new HttpClient
    {
        Timeout = TimeSpan.FromSeconds(TogetherConstants.TIMEOUT_SECS)
    })
    {
    }

    public TogetherClient(string apiKey, HttpClient httpClient, string? baseUrl = null)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new ArgumentException("API key is required", nameof(apiKey));
        }

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            baseUrl = BaseUrl;
        }

        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(baseUrl);
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