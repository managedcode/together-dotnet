using System.Net.Http.Json;
using Together.Models.Error;

namespace Together.Clients;

public abstract class BaseClient
{
    protected readonly HttpClient HttpClient;

    protected BaseClient(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    protected async Task<TResponse> SendRequestAsync<TRequest, TResponse>(string requestUri, TRequest request, CancellationToken cancellationToken)
    {
        var responseMessage = await HttpClient.PostAsJsonAsync(requestUri, request, cancellationToken);

        if (responseMessage.IsSuccessStatusCode)
        {
            return await HandleResponseAsync<TResponse>(responseMessage, cancellationToken);
        }

        var errorResponse = await responseMessage.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: cancellationToken);
        if (errorResponse?.Error != null)
        {
            throw new Exception(errorResponse.Error.Message);
        }

        var statusCode = responseMessage.StatusCode;
        var errorContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
        throw new Exception($"Request failed with status code {statusCode}: {errorContent}");
    }

    protected async Task<TResponse> SendRequestAsync<TResponse>(string requestUri, HttpMethod method, HttpContent? content, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, requestUri);
        if (content != null)
        {
            request.Content = content;
        }

        var responseMessage = await HttpClient.SendAsync(request, cancellationToken);

        if (responseMessage.IsSuccessStatusCode)
        {
            return await HandleResponseAsync<TResponse>(responseMessage, cancellationToken);
        }

        var errorResponse = await responseMessage.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: cancellationToken);
        if (errorResponse?.Error != null)
        {
            throw new Exception(errorResponse.Error.Message);
        }

        var statusCode = responseMessage.StatusCode;
        var errorContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
        throw new Exception($"Request failed with status code {statusCode}: {errorContent}");
    }

    private static async Task<TResponse> HandleResponseAsync<TResponse>(HttpResponseMessage responseMessage, CancellationToken cancellationToken)
    {
        if (responseMessage.IsSuccessStatusCode)
        {
            if (typeof(TResponse) == typeof(HttpResponseMessage) && responseMessage is TResponse response)
            {
                return response;
            }

            var result = await responseMessage.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
            return result!;
        }

        var errorResponse = await responseMessage.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: cancellationToken);
        if (errorResponse?.Error != null)
        {
            throw new Exception(errorResponse.Error.Message);
        }

        var statusCode = responseMessage.StatusCode;
        var errorContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
        throw new Exception($"Request failed with status code {statusCode}: {errorContent}");
    }
}