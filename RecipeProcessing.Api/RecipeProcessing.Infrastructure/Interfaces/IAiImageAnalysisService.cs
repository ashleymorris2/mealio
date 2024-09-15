namespace RecipeProcessing.Infrastructure.Interfaces;

public interface IAiImageAnalysisService
{
    Task<string> Process(Stream imageStream, string imageFileContentType);
}