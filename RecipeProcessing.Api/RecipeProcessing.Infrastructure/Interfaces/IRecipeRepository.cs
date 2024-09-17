using RecipeProcessing.Core.Entities;

namespace RecipeProcessing.Infrastructure.Interfaces
{
    public interface IRecipeRepository
    {
        Task AddAsync(Recipe recipe);
    }
}