namespace Together.Models.Finetune;

public enum FinetuneJobStatus
{
    Pending,
    Queued,
    Running,
    Compressing,
    Uploading,
    CancelRequested,
    Cancelled,
    Error,
    UserError,
    Completed
}