namespace RecipeProcessing.Core.Entities;

public class ImageHash
{
    public Guid Id { get; init; }
    public required string Hash { get; init; }
    public Guid RecipeId { get; init; }
    public Recipe? Recipe { get; set; } 
}