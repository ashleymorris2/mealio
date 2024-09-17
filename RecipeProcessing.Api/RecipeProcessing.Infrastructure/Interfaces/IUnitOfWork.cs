using Microsoft.EntityFrameworkCore.Storage;
using RecipeProcessing.Infrastructure.Repositories;

namespace RecipeProcessing.Infrastructure.Interfaces;

public interface IUnitOfWork
{
    public IRecipeRepository RecipeRepository { get; }
    public IImageHashRepository ImageHashRepository { get; }

    public Task<IDbContextTransaction> BeginTransactionAsync();
    Task SaveAsync();
}