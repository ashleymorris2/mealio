using System.Text;
using Microsoft.Extensions.Options;
using RecipeProcessing.Infrastructure.Integrations;
using RecipeProcessing.Infrastructure.Integrations.OpenAi;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Infrastructure.Services;

internal class OpenAiImageService(
    HttpClient httpClient,
    IOptions<OpenAiConfig> config,
    IRequestBuilder openAiRequestBuilder)
    : IImageService
{
    private readonly OpenAiConfig _openAiConfig = config.Value;

    public async Task<string> Process(Stream imageStream)
    {
        var model = _openAiConfig.gptModels[GptModel.Gpt4o];
        var apiKey = _openAiConfig.ApiKey;
        var endpoint = _openAiConfig.Endpoint;

        var request = openAiRequestBuilder.Build();

        var content = new StringContent(request, Encoding.UTF8, "application/json");
        var rawResponse = await httpClient.PostAsync("chat/completions", content);

        rawResponse.EnsureSuccessStatusCode();

        return await Task.FromResult("HELLO FROM THE IMAGE SERVICE");
    }
}