namespace RecipeProcessing.Infrastructure.Interfaces;

public interface IHashService
{
    public string ComputeFromStream(Stream inputStream);
}