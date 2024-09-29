using Microsoft.EntityFrameworkCore.Storage;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Infrastructure.Repositories;

internal class UnitOfWork(RecipeDbContext context) : IUnitOfWork
{
    private RecipeRepository? _recipeRepository;
    private IImageHashRepository? _imageHashRepository;

    public IRecipeRepository RecipeRepository => _recipeRepository ??= new RecipeRepository(context);
    public IImageHashRepository ImageHashRepository => _imageHashRepository ??= new ImageHashRepository(context);

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await context.Database.BeginTransactionAsync();
    }
}