using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using NJsonSchema.Generation;
using OpenAI.Chat;
using RecipeProcessing.Core.Entities;
using RecipeProcessing.Infrastructure.Integrations.OpenAi;
using RecipeProcessing.Infrastructure.Interfaces;


namespace RecipeProcessing.Infrastructure.Services;

internal class OpenAiImageService : IImageService
{
    private readonly OpenAiConfig _openAiConfig;
    private readonly ChatClient _chatClient;

    public OpenAiImageService(IOptions<OpenAiConfig> options)
    {
        _openAiConfig = options.Value;
        _chatClient = new ChatClient(_openAiConfig.gptModels[GptModel.Gpt4o], _openAiConfig.ApiKey);
    }

    public async Task<string> Process(Stream imageStream, string imageFileContentType)
    {
        var promptPath = Path.Combine(Directory.GetCurrentDirectory(), _openAiConfig.Prompts.ImageProcessing);
        var prompt = await File.ReadAllTextAsync(promptPath);
        var schema = JsonSchemaCache.GetOrGenerateSchemaForType<Recipe>();

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
