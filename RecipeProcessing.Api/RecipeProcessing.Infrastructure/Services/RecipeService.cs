using System.Text.Json;
using RecipeProcessing.Core.Entities;
using RecipeProcessing.Infrastructure.Interfaces;
using RecipeProcessing.Infrastructure.Repositories;

namespace RecipeProcessing.Infrastructure.Services;

public class RecipeService(IUnitOfWork unitOfWork) : IRecipeService
{
    public async Task SaveRecipeFromResult(string result, string imageHash)
    {
        if (string.IsNullOrWhiteSpace(result)) throw new ArgumentNullException(nameof(result));

        var recipe = JsonSerializer.Deserialize<Recipe>(result);
        if (recipe is null) throw new InvalidOperationException($"Deserialization returned null");
        
        await using var transaction = await unitOfWork.BeginTransactionAsync();

        try
        {
            await unitOfWork.RecipeRepository.AddAsync(recipe);
            await unitOfWork.SaveAsync();

            await unitOfWork.ImageHashRepository.AddAsync(new ImageHash() { Hash = imageHash, RecipeId = recipe.Id });
            await unitOfWork.SaveAsync();

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}