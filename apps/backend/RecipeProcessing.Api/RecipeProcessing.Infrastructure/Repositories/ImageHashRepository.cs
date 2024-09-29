using Microsoft.EntityFrameworkCore;
using RecipeProcessing.Core.Entities;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Infrastructure.Repositories;

internal class ImageHashRepository(RecipeDbContext context) : IImageHashRepository
{
    public async Task AddAsync(ImageHash imageHash)
    {
        await context.ImageHashes.AddAsync(imageHash);
    }

    public async Task<ImageHash?> GetAsync(string hash)
    {
        return await context.ImageHashes
            .Include(imageHash => imageHash.Recipe)
            .FirstOrDefaultAsync(imageHash => imageHash.Hash == hash);
    }
}