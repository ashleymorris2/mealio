using RecipeProcessing.Core.Entities;

namespace RecipeProcessing.Infrastructure.Interfaces;

public interface IRecipeService
{
    Task SaveRecipeFromResult(string recipe, string hash);
    public Task<Recipe?> GetRecipeByImageHash(string hash);
}