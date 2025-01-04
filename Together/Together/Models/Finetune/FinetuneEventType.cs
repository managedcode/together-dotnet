namespace Together.Models.Finetune;

public enum FinetuneEventType
{
    JobPending,
    JobStart,
    JobStopped,
    ModelDownloading,
    ModelDownloadComplete,
    TrainingDataDownloading,
    TrainingDataDownloadComplete,
    ValidationDataDownloading,
    ValidationDataDownloadComplete,
    WandbInit,
    TrainingStart,
    CheckpointSave,
    BillingLimit,
    EpochComplete,
    EvalComplete,
    TrainingComplete,
    ModelCompressing,
    ModelCompressionComplete,
    ModelUploading,
    ModelUploadComplete,
    JobComplete,
    JobError,
    JobUserError,
    CancelRequested,
    JobRestarted,
    Refund,
    Warning
}