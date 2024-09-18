using RecipeProcessing.Core.Entities;

namespace RecipeProcessing.Infrastructure.Interfaces;

internal interface IImageHashRepository
{
    public Task AddAsync(ImageHash imageHash);
    public Task<ImageHash?> GetAsync(string hash);
}