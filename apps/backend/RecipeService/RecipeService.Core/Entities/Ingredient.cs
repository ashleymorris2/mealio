// ReSharper disable ClassNeverInstantiated.Global

namespace RecipeProcessing.Core.Entities;

public class Ingredient
{
    public required string Name { get; set; }
    public double Quantity { get; set; }
    public string? Unit { get; set; }
}