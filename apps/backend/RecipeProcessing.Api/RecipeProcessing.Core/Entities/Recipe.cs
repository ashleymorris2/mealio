namespace RecipeProcessing.Core.Entities;

public class Recipe
{
    public int Id { get; init; }
    public required string Title { get; set; }
    public int Servings { get; set; }
    public TimeSpan PreparationTime { get; set; }
    public TimeSpan TotalTime { get; set; }
    public List<Ingredient> Ingredients { get; set; } = [];
    public List<InstructionStep> Instructions { get; set; } = [];
    public required NutritionalDetails NutritionPerServing { get; set; }
}