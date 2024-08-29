namespace RecipeProcessing.Core.Interfaces;

public interface IImageService
{
    Task<string> Process(Stream imageStream);
}