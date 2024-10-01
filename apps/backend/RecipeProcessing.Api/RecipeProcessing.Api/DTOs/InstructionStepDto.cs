namespace RecipeProcessing.Api.DTOs;

public class InstructionStepDto
{
    public int StepNumber { get; set; }
    public required string Description { get; set; }
}