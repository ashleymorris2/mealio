using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace RecipeProcessing.Infrastructure.Integrations.OpenAi;

internal class OpenAiConfigSetup(IConfiguration configuration) : IConfigureOptions<OpenAiConfig>
{
    public void Configure(OpenAiConfig options)
    {
        configuration.GetSection("OpenAi").Bind(options);

        if (string.IsNullOrWhiteSpace(options.ApiKey))
        {
            throw new InvalidOperationException(
                "The API key for OpenAI must be configured using an environment variable or User Secret."
            );
        }
    }
}