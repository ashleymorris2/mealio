using System.Text.Json;
using RecipeProcessing.Core.Entities;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Infrastructure.Services;

public class RecipeService(IUnitOfWork unitOfWork) : IRecipeService
{
    public async Task SaveRecipeFromResult(string result, string hash)
    {
        if (string.IsNullOrWhiteSpace(result)) throw new ArgumentNullException(nameof(result));

        var recipe = JsonSerializer.Deserialize<Recipe>(result);
        if (recipe is null) throw new InvalidOperationException($"Deserialization returned null");

        await using var transaction = await unitOfWork.BeginTransactionAsync();

        try
        {
            await unitOfWork.RecipeRepository.AddAsync(recipe);
            await unitOfWork.SaveAsync();

            await unitOfWork.ImageHashRepository.AddAsync(new ImageHash() { Hash = hash, RecipeId = recipe.Id });
            await unitOfWork.SaveAsync();

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Recipe?> GetRecipeByImageHash(string hash)
    {
        var imageHash = await unitOfWork.ImageHashRepository.GetAsync(hash);
        return imageHash?.Recipe;
    }
}