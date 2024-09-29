// ReSharper disable ClassNeverInstantiated.Global

namespace RecipeProcessing.Core.Entities;

public class InstructionStep
{
    public int StepNumber { get; set; }
    public required string Description { get; set; }
}

