using System.Net.Http;
using System.Threading.Tasks;
using Together.Models.Common;
using Together.Models.Finetune;

namespace Together.Clients;

public class FineTuneClient(HttpClient httpClient) : BaseClient(httpClient)
{
    
    //
    // public virtual async Task<FinetuneResponse> CreateAsync(FinetuneRequest request)
    // {
    //     var response = await Client.PostAsJsonAsync($"{BaseUrl}/fine-tunes", request);
    //     response.EnsureSuccessStatusCode();
    //     return await response.Content.ReadFromJsonAsync<FinetuneResponse>();
    // }
    //
    // public virtual async Task<FinetuneList> ListAsync()
    // {
    //     var response = await Client.GetAsync($"{BaseUrl}/fine-tunes");
    //     response.EnsureSuccessStatusCode();
    //     return await response.Content.ReadFromJsonAsync<FinetuneList>();
    // }
    //
    // public virtual async Task<FinetuneResponse> RetrieveAsync(string id)
    // {
    //     var response = await Client.GetAsync($"{BaseUrl}/fine-tunes/{id}");
    //     response.EnsureSuccessStatusCode();
    //     return await response.Content.ReadFromJsonAsync<FinetuneResponse>();
    // }
    //
    // public virtual async Task<FinetuneResponse> CancelAsync(string id)
    // {
    //     var response = await Client.PostAsync($"{BaseUrl}/fine-tunes/{id}/cancel", null);
    //     response.EnsureSuccessStatusCode();
    //     return await response.Content.ReadFromJsonAsync<FinetuneResponse>();
    // }
    //
    // public virtual async Task<FinetuneEvent[]> ListEventsAsync(string id)
    // {
    //     var response = await Client.GetAsync($"{BaseUrl}/fine-tunes/{id}/events");
    //     response.EnsureSuccessStatusCode();
    //     return await response.Content.ReadFromJsonAsync<FinetuneEvent[]>();
    // }
    //
    // public virtual async Task<FinetuneTrainingLimits> GetModelLimitsAsync(string model)
    // {
    //     var response = await Client.GetAsync($"{BaseUrl}/fine-tunes/models/limits?model={model}");
    //     response.EnsureSuccessStatusCode();
    //     return await response.Content.ReadFromJsonAsync<FinetuneTrainingLimits>();
    // }
    //
    // public virtual async Task<FinetuneDownloadResult> DownloadAsync(
    //     string id,
    //     string outputPath = null,
    //     int checkpointStep = -1,
    //     DownloadCheckpointType checkpointType = default)
    // {
    //     var url = $"{BaseUrl}/finetune/download?ft_id={id}";
    //     
    //     if (checkpointStep > 0)
    //     {
    //         url += $"&checkpoint_step={checkpointStep}";
    //     }
    //
    //     // Get finetune job details to determine training type
    //     var job = await RetrieveAsync(id);
    //
    //     // Configure checkpoint type based on training type
    //     if (job.TrainingType.Type == "full")
    //     {
    //         if (checkpointType != DownloadCheckpointType.Default)
    //         {
    //             throw new ArgumentException("Only DEFAULT checkpoint type is allowed for FullTrainingType");
    //         }
    //         url += "&checkpoint=modelOutputPath";
    //     }
    //     else if (job.TrainingType.Type == "lora")
    //     {
    //         if (checkpointType == DownloadCheckpointType.Default)
    //         {
    //             checkpointType = DownloadCheckpointType.Merged;
    //         }
    //
    //         url += $"&checkpoint={checkpointType.ToString().ToLowerInvariant()}";
    //     }
    //
    //     var response = await Client.GetAsync(url);
    //     response.EnsureSuccessStatusCode();
    //
    //     // Handle file download logic here
    //     var fileName = outputPath ?? job.OutputName ?? id;
    //     var fileSize = response.Content.Headers.ContentLength ?? 0;
    //
    //     // Save file content
    //     await using var fs = new FileStream(fileName, FileMode.Create);
    //     await response.Content.CopyToAsync(fs);
    //
    //     return new FinetuneDownloadResult
    //     {
    //         Object = "local",
    //         Id = id,
    //         CheckpointStep = checkpointStep,
    //         Filename = fileName,
    //         Size = (int)fileSize
    //     };
    // }
}