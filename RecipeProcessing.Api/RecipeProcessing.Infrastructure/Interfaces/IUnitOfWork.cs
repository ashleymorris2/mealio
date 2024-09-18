using Microsoft.EntityFrameworkCore.Storage;
using RecipeProcessing.Infrastructure.Repositories;

namespace RecipeProcessing.Infrastructure.Interfaces;

public interface IUnitOfWork
{
    internal IRecipeRepository RecipeRepository { get; }
    internal IImageHashRepository ImageHashRepository { get; }

    public Task<IDbContextTransaction> BeginTransactionAsync();
    Task SaveAsync();
}