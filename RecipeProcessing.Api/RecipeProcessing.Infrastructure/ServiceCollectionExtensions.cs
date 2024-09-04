using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using RecipeProcessing.Infrastructure.Integrations.OpenAi;
using RecipeProcessing.Infrastructure.Interfaces;
using RecipeProcessing.Infrastructure.Services;

namespace RecipeProcessing.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IConfigureOptions<OpenAiConfig>, OpenAiConfigSetup>();
        services.AddTransient<IImageService, OpenAiImageService>();
        
        return services;
    }
}