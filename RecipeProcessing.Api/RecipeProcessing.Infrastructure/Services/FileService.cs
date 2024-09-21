using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Infrastructure.Services;

public class FileService : IFileService
{
    public async Task<(string, string)> SaveTemporaryFileAsync(Stream fileStream, string fileExtension)
    {
        var tempPath = Path.GetTempPath();
        var tempFilePath = Path.Combine(tempPath, Guid.NewGuid() + fileExtension);

        await using var tempFileStream = new FileStream(tempFilePath, FileMode.Create);
        await fileStream.CopyToAsync(tempFileStream);

        return (tempFilePath, fileExtension); 
    }

    public void DeleteTemporaryFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);  // Delete the file after processing
        }
    }
}