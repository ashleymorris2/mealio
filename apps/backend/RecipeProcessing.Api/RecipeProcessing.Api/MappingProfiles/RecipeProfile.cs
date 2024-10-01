using AutoMapper;
using RecipeProcessing.Api.DTOs;
using RecipeProcessing.Core.Entities;

namespace RecipeProcessing.Api.MappingProfiles;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        CreateMap<Recipe, RecipeDto>();
        CreateMap<RecipeDto, Recipe>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}