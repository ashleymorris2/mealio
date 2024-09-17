using RecipeProcessing.Core.Entities;

namespace RecipeProcessing.Infrastructure.Repositories;

public interface IImageHashRepository
{
    public Task AddAsync(ImageHash imageHash);
}