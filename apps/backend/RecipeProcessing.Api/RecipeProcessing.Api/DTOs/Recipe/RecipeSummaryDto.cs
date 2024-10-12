namespace RecipeProcessing.Api.DTOs.Recipe;

public sealed record RecipeSummaryDto
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public TimeSpan TotalTime { get; init; }
}