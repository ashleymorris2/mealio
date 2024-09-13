using System.Text.Json;
using RecipeProcessing.Core.Entities;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Infrastructure.Services;

public class RecipeService : IRecipeService
{
    private readonly IRecipeRepository _repository;

    public RecipeService(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task SaveRecipeFromResult(string result)
    {
        if (string.IsNullOrWhiteSpace(result)) throw new ArgumentNullException(nameof(result));
        var recipe = JsonSerializer.Deserialize<Recipe>(result);
        if (recipe is null) throw new InvalidOperationException($"Deserialization returned null");

        await _repository.AddRecipeAsync(recipe);
        await _repository.SaveChangesAsync();
    }
}