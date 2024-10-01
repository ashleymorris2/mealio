using Microsoft.EntityFrameworkCore;
using RecipeProcessing.Core.Entities;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Infrastructure.Repositories;

internal class RecipeRepository(RecipeDbContext context) : IRecipeRepository
{
    public async Task AddAsync(Recipe recipe)
    {
        await context.Recipes.AddAsync(recipe);
    }

    public async Task<Recipe?> GetByIdAsync(Guid id)
    {
        return await context.Recipes.FindAsync(id);
    }

    public async Task<IEnumerable<Recipe>> GetAllByPage(int pageNumber, int pageSize)
    {
        return await context.Recipes
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public void Update(Recipe recipe)
    {
        context.Entry(recipe).State = EntityState.Modified;
    }

    public async Task<int> Count()
    {
        return await context.Recipes.CountAsync();
    }
}