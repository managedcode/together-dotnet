using System.Net.Http;
using Together.Models.CodeInterpreter;

namespace Together.Clients;

public class CodeInterpreterClient(HttpClient httpClient) : BaseClient(httpClient)
{
    public async Task<ExecuteResponse> RunAsync(CodeInterpreterRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<CodeInterpreterRequest, ExecuteResponse>("/tci/execute", request, cancellationToken);
    }
}
