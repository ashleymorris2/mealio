namespace RecipeProcessing.Core.Entities;

public class RecipeImageHash
{
    public int Id { get; set; }
    public required string Hash { get; set; }
    
    public int RecipeId { get; set; }
    public required Recipe Recipe { get; set; } 
}