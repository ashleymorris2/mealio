using RecipeProcessing.Core.Interfaces;

namespace RecipeProcessing.Infrastructure.Services;

public class ImageService : IImageService
{
    public async Task<string> Process(Stream imageStream)
    {
        return await Task.FromResult("HELLO FROM THE IMAGE SERVICE");
    }
}
