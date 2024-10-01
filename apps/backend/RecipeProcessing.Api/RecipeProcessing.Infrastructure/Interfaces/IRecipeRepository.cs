using RecipeProcessing.Core.Entities;

namespace RecipeProcessing.Infrastructure.Interfaces;

internal interface IRecipeRepository
{
    Task AddAsync(Recipe recipe);
    Task<Recipe?> GetByIdAsync(Guid id);
    Task<IEnumerable<Recipe>> GetAllByPage(int page, int pageSize);
    void Update(Recipe recipe);
    Task<int> Count();
}