using Microsoft.Extensions.Options;
using OpenAI.Chat;
using RecipeProcessing.Core.Entities;
using RecipeProcessing.Infrastructure.Caching;
using RecipeProcessing.Infrastructure.Integrations.OpenAi;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Infrastructure.Services;

internal class OpenAiAiImageAnalysisService : IAiImageAnalysisService
{
    private readonly OpenAiConfig _openAiConfig;
    private readonly ChatClient _chatClient;
    private readonly JsonSchemaCache _jsonSchemaCache;

    public OpenAiAiImageAnalysisService(IOptions<OpenAiConfig> options, JsonSchemaCache jsonSchemaCache)
    {
        _openAiConfig = options.Value;
        _chatClient = new ChatClient(_openAiConfig.gptModels[GptModel.Gpt4o], _openAiConfig.ApiKey);
        _jsonSchemaCache = jsonSchemaCache;
    }

    public async Task<string> Process(Stream imageStream, string imageFileContentType)
    {
        var promptPath = Path.Combine(Directory.GetCurrentDirectory(), _openAiConfig.Prompts.ImageProcessing);
        var prompt = await File.ReadAllTextAsync(promptPath);
        var schema = _jsonSchemaCache.GetOrGenerateSchemaForType<Recipe>();

        ChatCompletionOptions options = new()
        {
            ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                name: "recipe_details",
                jsonSchema: BinaryData.FromString(schema.ToJson()),
                strictSchemaEnabled: true
            )
        };

        BinaryData imageBytes = await BinaryData.FromStreamAsync(imageStream);
        List<ChatMessage> messages =
        [
            new UserChatMessage(
                ChatMessageContentPart.CreateTextMessageContentPart(prompt),
                ChatMessageContentPart.CreateImageMessageContentPart(imageBytes, imageFileContentType))
        ];

        ChatCompletion chatCompletion = await _chatClient.CompleteChatAsync(messages, options);

        return chatCompletion.Content[0].Text;
    }
}
