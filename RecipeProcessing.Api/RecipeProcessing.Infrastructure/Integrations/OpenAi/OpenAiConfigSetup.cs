using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace RecipeProcessing.Infrastructure.Integrations.OpenAi;

internal class OpenAiConfigSetup(IConfiguration configuration) : IConfigureOptions<OpenAiConfig>
{
    public void Configure(OpenAiConfig openAiConfig)
    {
        configuration.GetSection("OpenAi").Bind(openAiConfig);

        if (string.IsNullOrWhiteSpace(openAiConfig.ApiKey))
        {
            throw new InvalidOperationException(
                "The API key for OpenAI must be configured using an environment variable or User Secret."
            );
        }
    }
}