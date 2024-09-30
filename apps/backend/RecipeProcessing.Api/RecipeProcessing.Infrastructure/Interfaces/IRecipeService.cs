using RecipeProcessing.Core.Entities;

namespace RecipeProcessing.Infrastructure.Interfaces;

public interface IRecipeService
{
    Task CreateRecipeFromResult(string recipe, string hash);
    Task<Recipe> CreateRecipeAsync(Recipe recipe);
    public Task<Recipe?> GetRecipeByImageHash(string hash);
    Task<Recipe?> GetRecipeByIdAsync(Guid id);
    Task UpdateRecipeAsync(Guid id, Recipe recipe);
}