using RecipeProcessing.Core.Entities;

namespace RecipeProcessing.Infrastructure.Interfaces
{
    public interface IRecipeRepository
    {
        Task AddRecipeAsync(Recipe recipe);
        Task SaveChangesAsync();
    }
}