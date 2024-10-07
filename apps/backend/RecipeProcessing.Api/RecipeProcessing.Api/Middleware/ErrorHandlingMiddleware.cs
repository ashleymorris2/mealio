using System.Net;
using Newtonsoft.Json;

namespace RecipeProcessing.Api.Middleware;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception has occurred.");
            await HandleExceptionAsync(context);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context)
    {
        var response = new
        {
            error = "An error has occurred while processing your request."
        };
        
        var errorResponse = JsonConvert.SerializeObject(response);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        
        return context.Response.WriteAsync(errorResponse);
    }
}