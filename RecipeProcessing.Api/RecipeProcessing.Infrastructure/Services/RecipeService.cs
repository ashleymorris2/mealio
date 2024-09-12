using RecipeProcessing.Core.Entities;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Infrastructure.Services;

public class RecipeService(IRecipeRepository repository) : IRecipeService
{
    public async Task SaveRecipeFromResult(string recipe)
    {
        
        // await _repository.AddRecipeAsync(recipe);
        // await repository.SaveChangesAsync();
    }
}