using Microsoft.Extensions.Options;
using RecipeProcessing.Core.Interfaces;
using RecipeProcessing.Infrastructure.Integrations.OpenAi;

namespace RecipeProcessing.Infrastructure.Services;

public class OpenAiImageService : IImageService
{
    private readonly OpenAiConfig _openAiConfig;

    internal OpenAiImageService(IOptions<OpenAiConfig> config)
    {
        _openAiConfig = config.Value;
    }

    public async Task<string> Process(Stream imageStream)
    {
        // var model = _openAiConfig.gptModels[GptModel.Gpt4o];
        
        
        return await Task.FromResult("HELLO FROM THE IMAGE SERVICE");
    }
}
