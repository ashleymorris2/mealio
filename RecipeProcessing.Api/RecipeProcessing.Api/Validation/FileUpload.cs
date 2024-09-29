using System.ComponentModel.DataAnnotations;

namespace RecipeProcessing.Api.Validation;

public class FileUpload : IValidatableObject
{
    private const int MaxFileSize = 8;//Mb
    public required IFormFile? ImageFile { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ImageFile == null || ImageFile.Length == 0)
        {
            yield return new ValidationResult("No file uploaded.",
                [nameof(ImageFile)]);
            
            yield break;
        }

        var contentType = ImageFile?.ContentType;
        var allowedTypes = new[] { "image/jpeg", "image/png" };
        if (!allowedTypes.Contains(contentType))
        {
            yield return new ValidationResult("Invalid file type. Only JPEG and PNG files are allowed.",
                [nameof(ImageFile)]);
        }

        if (ImageFile?.Length > MaxFileSize * 1024 * 1024)
        {
            yield return new ValidationResult($"File size exceeds the {MaxFileSize}MB limit.",
                [nameof(ImageFile)]);
        }
    }
}