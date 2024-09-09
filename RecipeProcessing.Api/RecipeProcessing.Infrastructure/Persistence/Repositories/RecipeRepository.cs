using RecipeProcessing.Core.Entities;
using RecipeProcessing.Core.Interfaces;

namespace RecipeProcessing.Infrastructure.Persistence.Repositories;

public class RecipeRepository(RecipeDbContext context) : IRecipeRepository
{
    public async Task AddRecipeAsync(Recipe recipe)
    {
        await context.Recipes.AddAsync(recipe);
    }
}