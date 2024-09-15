namespace RecipeProcessing.Infrastructure.Interfaces;

public interface IHashService
{
    public string ComputeHashFromStream(Stream inputStream);
}