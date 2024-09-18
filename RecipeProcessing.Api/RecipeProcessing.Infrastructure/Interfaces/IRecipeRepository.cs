using RecipeProcessing.Core.Entities;

namespace RecipeProcessing.Infrastructure.Interfaces
{
    internal interface IRecipeRepository
    {
        Task AddAsync(Recipe recipe);
    }
}