using System.Text.Json;
using RecipeProcessing.Core.Entities;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Infrastructure.Services;

public class RecipeService(IRecipeRepository repository) : IRecipeService
{
    public async Task SaveRecipeFromResult(string result)
    {
        if (string.IsNullOrWhiteSpace(result)) throw new ArgumentNullException(nameof(result));
        
        var recipe = JsonSerializer.Deserialize<Recipe>(result);
        if (recipe is null) throw new InvalidOperationException($"Deserialization returned null");

        await repository.AddRecipeAsync(recipe);
        await repository.SaveChangesAsync();
    }
}