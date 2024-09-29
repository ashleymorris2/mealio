// ReSharper disable ClassNeverInstantiated.Global

namespace RecipeProcessing.Core.Entities;

public class NutritionalDetails
{
    public int Calories { get; set; }
    public int Protein { get; set; } 
    public int Fat { get; set; }     
    public int SaturatedFat { get; set; }  
    public int Fiber { get; set; }   
    public int Carbohydrates { get; set; } 
    public int Sugars { get; set; }   
    public double Salt { get; set; }  
}