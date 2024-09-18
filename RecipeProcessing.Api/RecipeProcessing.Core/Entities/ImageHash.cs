namespace RecipeProcessing.Core.Entities;

public class ImageHash
{
    public int Id { get; init; }
    public required string Hash { get; init; }
    public int RecipeId { get; init; }
    public Recipe? Recipe { get; set; } 
}