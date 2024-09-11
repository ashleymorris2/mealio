using RecipeProcessing.Core.Entities;

namespace RecipeProcessing.Core.Interfaces
{
    public interface IRecipeRepository
    {
        Task AddRecipeAsync(Recipe recipe);
    }
}