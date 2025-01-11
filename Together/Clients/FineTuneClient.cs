using Together.Models.Finetune;

namespace Together.Clients;

public class FineTuneClient(HttpClient httpClient) : BaseClient(httpClient)
{
    public virtual async Task<FinetuneResponse> CreateAsync(FinetuneRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<FinetuneRequest, FinetuneResponse>("/fine-tunes", request, cancellationToken);
    }

    public virtual async Task<FinetuneList> ListAsync(CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<FinetuneList>("/fine-tunes", HttpMethod.Get, null, cancellationToken);
    }

    public virtual async Task<FinetuneResponse> RetrieveAsync(string id, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<FinetuneResponse>($"/fine-tunes/{id}", HttpMethod.Get, null, cancellationToken);
    }

    public virtual async Task<FinetuneResponse> CancelAsync(string id, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<FinetuneResponse>($"/fine-tunes/{id}/cancel", HttpMethod.Post, null, cancellationToken);
    }

    public virtual async Task<FinetuneListEvents> ListEventsAsync(string id, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<FinetuneListEvents>($"/fine-tunes/{id}/events", HttpMethod.Get, null, cancellationToken);
    }

    public virtual async Task<FinetuneTrainingLimits> GetModelLimitsAsync(string model, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<FinetuneTrainingLimits>($"/fine-tunes/models/limits?model={model}", HttpMethod.Get, null, cancellationToken);
    }

    public virtual async Task<FinetuneDownloadResult> DownloadAsync(string id, string? outputPath = null, int checkpointStep = -1,
        DownloadCheckpointType? checkpointType = null, CancellationToken cancellationToken = default)
    {
        var url = $"/finetune/download?ft_id={id}";
        if (checkpointStep > 0)
        {
            url += $"&checkpoint_step={checkpointStep}";
        }

        checkpointType ??= DownloadCheckpointType.Default;

        var job = await RetrieveAsync(id, cancellationToken);
        url = ConfigureCheckpointUrl(url, job.TrainingType, checkpointType.Value);

        var response = await HttpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        var fileName = outputPath ?? job.OutputName ?? id;
        var fileSize = response.Content.Headers.ContentLength ?? 0;

        await using var fs = new FileStream(fileName, FileMode.Create);
        await response.Content.CopyToAsync(fs, cancellationToken);

        return new FinetuneDownloadResult
        {
            Object = "local",
            Id = id,
            CheckpointStep = checkpointStep,
            Filename = fileName,
            Size = (int)fileSize
        };
    }

    private static string ConfigureCheckpointUrl(string url, TrainingType trainingType, DownloadCheckpointType checkpointType)
    {
        if (trainingType.Type == "full")
        {
            if (checkpointType != DownloadCheckpointType.Default)
            {
                throw new ArgumentException("Only DEFAULT checkpoint type is allowed for FullTrainingType");
            }

            return url + "&checkpoint=modelOutputPath";
        }

        if (trainingType.Type == "lora")
        {
            checkpointType = checkpointType == DownloadCheckpointType.Default ? DownloadCheckpointType.Merged : checkpointType;
            return url + $"&checkpoint={checkpointType.ToString().ToLowerInvariant()}";
        }

        throw new ArgumentException($"Unsupported training type: {trainingType.Type}");
    }
}