using RecipeProcessing.Core.Entities;
using RecipeProcessing.Infrastructure.Interfaces;
using RecipeProcessing.Infrastructure.Persistence;

namespace RecipeProcessing.Infrastructure.Repositories;

public class RecipeRepository(RecipeDbContext context) : IRecipeRepository
{
    public async Task AddRecipeAsync(Recipe recipe)
    {
        await context.Recipes.AddAsync(recipe);
    }

    public async Task SaveChangesAsync()
    {
        // await context.SaveChangesAsync();
    }
}