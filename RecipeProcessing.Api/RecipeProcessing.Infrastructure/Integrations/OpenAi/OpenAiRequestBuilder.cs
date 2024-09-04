using Microsoft.Extensions.Options;
using OpenAI.Chat;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Infrastructure.Integrations.OpenAi;

internal class OpenAiRequestBuilder(IOptions<OpenAiConfig> options) : IRequestBuilder
{
    readonly OpenAiConfig _openAiConfig = options.Value;

    public async Task<string> Build()
    {
        return await BuildRequest();
    }
    
    private async Task<string> BuildRequest()
    {
        ChatClient chatClient = new ChatClient(_openAiConfig.gptModels[GptModel.Gpt4o],_openAiConfig.ApiKey);
        ChatCompletion completion = await chatClient.CompleteChatAsync("Say 'this is a test.'");

        return completion.Content[0].Text;
    }
}