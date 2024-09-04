using Microsoft.Extensions.Options;
using OpenAI.Chat;
using RecipeProcessing.Infrastructure.Integrations.OpenAi;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Infrastructure.Services;

internal class OpenAiImageService(IOptions<OpenAiConfig> options) : IImageService
{
    readonly OpenAiConfig _openAiConfig = options.Value;
    
    public async Task<string> Process(Stream imageStream, string imageFileContentType)
   {
       BinaryData imageBytes = await BinaryData.FromStreamAsync(imageStream);
       List<ChatMessage> messages = [
           new UserChatMessage(
               ChatMessageContentPart.CreateTextMessageContentPart("Please describe the following image."),
               ChatMessageContentPart.CreateImageMessageContentPart(imageBytes, imageFileContentType))
       ];
       
       ChatClient chatClient = new ChatClient(_openAiConfig.gptModels[GptModel.Gpt4o],_openAiConfig.ApiKey);
       ChatCompletion chatCompletion = await chatClient.CompleteChatAsync(messages);

       return chatCompletion.Content[0].Text;
   }
}