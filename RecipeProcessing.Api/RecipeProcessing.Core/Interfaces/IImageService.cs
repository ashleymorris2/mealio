namespace RecipeProcessing.Core.Interfaces;

public interface IImageService
{
    String Process(Stream imageStream);
}