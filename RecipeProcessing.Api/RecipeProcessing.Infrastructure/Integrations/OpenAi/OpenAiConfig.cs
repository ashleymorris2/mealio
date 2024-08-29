namespace RecipeProcessing.Infrastructure.Integrations.OpenAi;

// ReSharper disable twice InconsistentNaming
internal enum GptModel
{
    Gpt3_5Turbo,
    Gpt4,
    Gpt4o
}

internal class OpenAiConfig
{
    public readonly Dictionary<GptModel, string> gptModels = new()
    {
        { GptModel.Gpt3_5Turbo, "gpt-3.5-turbo" },
        { GptModel.Gpt4, "gpt-4" },
        { GptModel.Gpt4o, "gpt-4o" }
    };
}