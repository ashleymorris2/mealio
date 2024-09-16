using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using RecipeProcessing.Infrastructure.Caching;
using RecipeProcessing.Infrastructure.Integrations.OpenAi;
using RecipeProcessing.Infrastructure.Interfaces;
using RecipeProcessing.Infrastructure.Repositories;
using RecipeProcessing.Infrastructure.Services;

namespace RecipeProcessing.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IConfigureOptions<OpenAiConfig>, OpenAiConfigSetup>();
        services.AddSingleton<JsonSchemaCache>();
        
        services.AddScoped<IRecipeRepository, RecipeRepository>();
        
        services.AddTransient<IAiImageAnalysisService, OpenAiAiImageAnalysisService>();
        services.AddTransient<IRecipeService, RecipeService>();
        services.AddTransient<IHashService, HashService>();
        
        return services;
    }

    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RecipeDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        return services;
    }
}