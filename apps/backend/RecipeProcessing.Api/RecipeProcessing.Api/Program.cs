using Microsoft.AspNetCore.Diagnostics;
using RecipeProcessing.Api.Middleware;
using RecipeProcessing.Api.Workers;
using RecipeProcessing.Infrastructure;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    #region Configure Services

    builder.Services.AddSerilog();

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Add services to the container.
    builder.Services.AddControllers();

    builder.Configuration
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
        .AddUserSecrets<Program>(optional: true)
        .AddEnvironmentVariables();

    // Add Swagger/OpenAPI support
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // // Add HttpClient
    builder.Services.AddHttpClient();

    //Infrastructure
    builder.Services.AddInfrastructure();

    //Database
    builder.Services.ConfigureDatabase(builder.Configuration);

    //Redis
    builder.Services.AddRedis(builder.Configuration);

    //Workers
    builder.Services.AddHostedService<ImageProcessingWorker>();

    #endregion

    var app = builder.Build();

    #region Configure Middleware
    
    app.UseSerilogRequestLogging();
    
    app.UseMiddleware<ErrorHandlingMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
    // Redirect HTTP requests to HTTPS
    app.UseHttpsRedirection();

    app.UseAuthorization();

    // Map controller routes
    app.MapControllers();

    #endregion

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly - Unhandled exception");
}
finally
{
    Log.CloseAndFlush();
}