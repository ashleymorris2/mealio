namespace RecipeProcessing.Infrastructure.Models;

public class ImageProcessingTask
{
    public required string FilePath { get; init; }
    public required string ImageHash { get; init; }
    public required string MimeType { get; init; }
    public required string StreamEntryId { get; init; }
}