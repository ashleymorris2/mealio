namespace RecipeProcessing.Api.DTOs;

public class RecipeDto
{
    public required string Title { get; set; }
    public int Servings { get; set; }
    public TimeSpan PreparationTime { get; set; }
    public TimeSpan TotalTime { get; set; }
    public List<IngredientDto> Ingredients { get; set; } = [];
    public List<InstructionStepDto> Instructions { get; set; } = [];
    public required NutritionalDetailsDto NutritionPerServing { get; set; }
}