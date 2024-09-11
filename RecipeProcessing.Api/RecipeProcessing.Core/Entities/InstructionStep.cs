// ReSharper disable ClassNeverInstantiated.Global
using Microsoft.EntityFrameworkCore;

namespace RecipeProcessing.Core.Entities;

[Owned]
public class InstructionStep
{
    public int StepNumber { get; set; }
    public required string Description { get; set; }
}

