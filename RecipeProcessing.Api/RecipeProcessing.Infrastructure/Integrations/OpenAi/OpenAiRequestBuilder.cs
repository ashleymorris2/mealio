using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Infrastructure.Integrations.OpenAi;

internal class OpenAiRequestBuilder(IOptions<OpenAiConfig> config) : IRequestBuilder
{
    private readonly OpenAiConfig _openAiConfig = config.Value;

    public string Build()
    {
        var model = _openAiConfig.gptModels[GptModel.Gpt4o];

        var request = new
        {
            model, 
            messages = new[]
            {
                new { role = "system", content = "You are a helpful assistant." },
                new { role = "user", content = "Hello!" }
            }
        };

        return JsonConvert.SerializeObject(request);
    }
}