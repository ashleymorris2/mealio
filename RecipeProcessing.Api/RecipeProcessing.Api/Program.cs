using RecipeProcessing.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

#region Configure Services

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Infrastructure
builder.Services.AddInfrastructure();


// Add HttpClient
builder.Services.AddHttpClient();

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