using Microsoft.SemanticKernel;

namespace Together.SemanticKernel.Converters;

public static class ChatOptionsConverter
{
    public static Dictionary<string, object> Convert(PromptExecutionSettings settings)
    {
        var result = new Dictionary<string, object>();

        if (settings?.ExtensionData == null)
        {
            return result;
        }

        foreach (var kvp in settings.ExtensionData)
        {
            result[kvp.Key] = kvp.Value;
        }

        return result;
    }
}
