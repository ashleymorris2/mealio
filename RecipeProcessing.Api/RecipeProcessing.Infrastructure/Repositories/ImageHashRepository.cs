using RecipeProcessing.Core.Entities;

namespace RecipeProcessing.Infrastructure.Repositories;

public class ImageHashRepository(RecipeDbContext context) : IImageHashRepository
{
    public async Task AddAsync(ImageHash imageHash)
    {
        await context.ImageHashes.AddAsync(imageHash);
    }
}