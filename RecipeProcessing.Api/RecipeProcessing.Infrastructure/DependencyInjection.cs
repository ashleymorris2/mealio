using Microsoft.Extensions.DependencyInjection;
using RecipeProcessing.Core.Interfaces;
using RecipeProcessing.Infrastructure.Integrations.OpenAi;
using RecipeProcessing.Infrastructure.Services;

namespace RecipeProcessing.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IImageService>(s => new OpenAiImageService(new OpenAiConfig()));

        return services;
    }
}