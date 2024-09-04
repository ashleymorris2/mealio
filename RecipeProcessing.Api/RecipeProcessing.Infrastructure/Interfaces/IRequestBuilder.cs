namespace RecipeProcessing.Infrastructure.Interfaces;

internal interface IRequestBuilder
{
     Task<string> Build();
}