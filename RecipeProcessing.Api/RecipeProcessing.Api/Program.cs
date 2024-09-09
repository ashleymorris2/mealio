using RecipeProcessing.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

#region Configure Services

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

#endregion

var app = builder.Build();

#region Configure Middleware
    
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