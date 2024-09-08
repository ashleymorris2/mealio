// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable twice InconsistentNaming

namespace RecipeProcessing.Infrastructure.Integrations.OpenAi;

internal enum GptModel
{
    Gpt3_5Turbo,
    Gpt4,
    Gpt4o
}

// Nested class for Prompts
internal class PromptsConfig
{
    public string ImageProcessing { get; set; } = string.Empty;
}

internal class OpenAiConfig()
{
    public string ApiKey { get; set; } = string.Empty;
    public PromptsConfig Prompts { get; set; } = new();

    public readonly Dictionary<GptModel, string> gptModels = new()
    {
        { GptModel.Gpt3_5Turbo, "gpt-3.5-turbo" },
        { GptModel.Gpt4, "gpt-4" },
        { GptModel.Gpt4o, "gpt-4o" }
    };
}