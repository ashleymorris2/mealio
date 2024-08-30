using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RecipeProcessing.Core.Interfaces;
using RecipeProcessing.Infrastructure.Integrations.OpenAi;
using RecipeProcessing.Infrastructure.Services;

namespace RecipeProcessing.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IConfigureOptions<OpenAiConfig>, OpenAiConfigSetup>();
        services.AddTransient<IImageService>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<OpenAiConfig>>();
            return new OpenAiImageService(config);
        });

        return services;
    }
}