using Microsoft.Extensions.Options;
using OpenAI.Chat;
using RecipeProcessing.Infrastructure.Integrations.OpenAi;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Infrastructure.Services;

internal class OpenAiImageService(IOptions<OpenAiConfig> options) : IImageService
{
    readonly OpenAiConfig _openAiConfig = options.Value;
    
    public async Task<string> Process(Stream imageStream)
   {
       ChatClient chatClient = new ChatClient(_openAiConfig.gptModels[GptModel.Gpt4o],_openAiConfig.ApiKey);
       ChatCompletion completion = await chatClient.CompleteChatAsync("Say 'this is a test.'");

       return completion.Content[0].Text;
   }
}