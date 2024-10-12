using AutoMapper;
using RecipeProcessing.Api.DTOs.Recipe;
using RecipeProcessing.Core.Entities;
// ReSharper disable UnusedType.Global

namespace RecipeProcessing.Api.MappingProfiles;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        CreateMap<Recipe, RecipeDto>();
        CreateMap<RecipeDto, Recipe>().ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
        CreateMap<Recipe, RecipeSummaryDto>();
    }
}

