
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace RecipeProcessing.Infrastructure.Integrations.OpenAi;

internal class OpenAiConfigSetup : IConfigureOptions<OpenAiConfig>
{
    private readonly IConfiguration _configuration;

    public OpenAiConfigSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(OpenAiConfig options)
    {
        // Bind the "OpenAi" section from appsettings.json to OpenAiConfig
        _configuration.GetSection("OpenAi").Bind(options);
    }
}