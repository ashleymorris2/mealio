using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using RecipeProcessing.Infrastructure.Integrations.OpenAi;
using RecipeProcessing.Infrastructure.Interfaces;
using RecipeProcessing.Infrastructure.Persistence;
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

    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RecipeDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        return services;
    }
}