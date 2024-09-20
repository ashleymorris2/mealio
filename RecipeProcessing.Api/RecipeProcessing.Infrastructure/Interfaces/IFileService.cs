namespace RecipeProcessing.Infrastructure.Interfaces;

public interface IFileService
{
    Task<string> SaveTemporaryFileAsync(Stream fileStream, string fileExtension);
    void DeleteTemporaryFile(string filePath);
}