using Together.Models.Finetune;

namespace Together;

public static class TogetherConstants
{
    // Session constants
    public const int TIMEOUT_SECS = 600;
    public const int MAX_SESSION_LIFETIME_SECS = 180;
    public const int MAX_CONNECTION_RETRIES = 2;
    public const int MAX_RETRIES = 5;
    public const double INITIAL_RETRY_DELAY = 0.5;
    public const double MAX_RETRY_DELAY = 8.0;

    // API defaults
    public const string BASE_URL = "https://api.together.xyz/v1";

    // Download defaults
    public const int DOWNLOAD_BLOCK_SIZE = 10 * 1024 * 1024; // 10 MB
    public const bool DISABLE_TQDM = false;

    // Messages
    public const string MISSING_API_KEY_MESSAGE = @"TOGETHER_API_KEY not found.
Please set it as an environment variable or set it as together.api_key
Find your TOGETHER_API_KEY at https://api.together.xyz/settings/api-keys";

    // Minimum number of samples required for fine-tuning file
    public const int MIN_SAMPLES = 1;

    // The number of bytes in a gigabyte, used to convert bytes to GB for readable comparison
    public const long NUM_BYTES_IN_GB = 1L << 30;

    // Maximum number of GB sized files we support finetuning for
    public const double MAX_FILE_SIZE_GB = 4.9;

    // Expected columns for Parquet files
    public static readonly List<string> PARQUET_EXPECTED_COLUMNS = new() { "input_ids", "attention_mask", "labels" };

    // JSONL required columns map
    public static readonly Dictionary<DatasetFormat, List<string>> JSONL_REQUIRED_COLUMNS_MAP = new()
    {
        { DatasetFormat.General, new List<string> { "text" } },
        { DatasetFormat.Conversation, new List<string> { "messages" } },
        { DatasetFormat.Instruction, new List<string> { "prompt", "completion" } }
    };

    public static readonly List<string> REQUIRED_COLUMNS_MESSAGE = new() { "role", "content" };
    public static readonly List<string> POSSIBLE_ROLES_CONVERSATION = new() { "system", "user", "assistant" };
}