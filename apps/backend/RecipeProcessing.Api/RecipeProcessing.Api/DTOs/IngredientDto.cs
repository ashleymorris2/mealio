namespace RecipeProcessing.Api.DTOs;

public class IngredientDto
{
    public required string Name { get; set; }
    public double Quantity { get; set; }
    public string? Unit { get; set; }
}