namespace RecipeProcessing.Core.Entities;

public class Recipe
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public int Servings { get; set; }
    public TimeSpan PreparationTime { get; set; }
    public TimeSpan TotalTime { get; set; } 
    public  ICollection<Ingredient>? Ingredients { get; set; }
    public  ICollection<InstructionStep>? Instructions { get; set; }
    public required NutritionalDetails NutritionPerServing { get; set; }
}