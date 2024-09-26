using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RecipeProcessing.Infrastructure.Caching;
using RecipeProcessing.Infrastructure.Integrations.OpenAi;
using RecipeProcessing.Infrastructure.Interfaces;
using RecipeProcessing.Infrastructure.Queues;
using RecipeProcessing.Infrastructure.Repositories;
using RecipeProcessing.Infrastructure.Services;
using StackExchange.Redis;

namespace RecipeProcessing.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IConfigureOptions<OpenAiConfig>, OpenAiConfigSetup>();
        services.AddSingleton<JsonSchemaCache>();

        services.AddScoped<IRecipeService, RecipeService>();

        services.AddTransient<IAiImageAnalysisService, OpenAiAiImageAnalysisService>();
        services.AddTransient<IHashService, HashService>();
        services.AddTransient<IFileService, FileService>();

        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetValue<string>("ConnectionStrings:RedisConnection");
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(
                redisConnectionString ?? throw new InvalidOperationException(nameof(redisConnectionString))
            )
        );
        
        services.Configure<RedisQueueOptions>(configuration.GetSection("RedisQueue"));
        services.AddSingleton<IQueueService, RedisQueueService>();

        return services;
    }

    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RecipeDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DatabaseConnection")));

        services.AddScoped<IRecipeRepository, RecipeRepository>();
        services.AddScoped<IImageHashRepository, ImageHashRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}