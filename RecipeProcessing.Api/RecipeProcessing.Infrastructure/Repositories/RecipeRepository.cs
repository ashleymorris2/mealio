using RecipeProcessing.Core.Entities;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Infrastructure.Repositories;

public class RecipeRepository(RecipeDbContext context) : IRecipeRepository
{
    public async Task AddAsync(Recipe recipe)
    {
        await context.Recipes.AddAsync(recipe);
    }

}