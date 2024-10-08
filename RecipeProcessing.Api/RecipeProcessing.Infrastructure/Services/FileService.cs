using System.Text.RegularExpressions;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Infrastructure.Services;

public partial class FileService : IFileService
{
    [GeneratedRegex("^image/")]
    private static partial Regex ImageRegexPattern();

    public async Task<(string, string)> SaveTemporaryFileAsync(Stream fileStream, string mimeType)
    {
        var tempPath = Path.GetTempPath();

        if (!Directory.Exists(tempPath))
        {
            Directory.CreateDirectory(tempPath);
        }

        // Replaces "image/" in a given mimetype with a . so image/png becomes .png
        var fileExtension = ImageRegexPattern().Replace(mimeType, ".");
        var tempFilePath = Path.Combine(tempPath, $"{Guid.NewGuid()}{fileExtension}");

        await using var tempFileStream = new FileStream(tempFilePath, FileMode.Create);
        await fileStream.CopyToAsync(tempFileStream);

        return (tempFilePath, mimeType);
    }

    public void DeleteTemporaryFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}