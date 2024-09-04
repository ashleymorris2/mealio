namespace RecipeProcessing.Infrastructure.Interfaces;

public interface IImageService
{
    Task<string> Process(Stream imageStream, string imageFileContentType);
}