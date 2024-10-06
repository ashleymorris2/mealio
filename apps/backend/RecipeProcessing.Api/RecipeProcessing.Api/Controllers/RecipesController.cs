using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecipeProcessing.Api.DTOs;
using RecipeProcessing.Core.Entities;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController(IRecipeService recipeService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recipe>>> GetAll(int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest("Page number and page size must be greater than 0.");
        }

        if (pageSize > 100)
        {
            return BadRequest("Page size must be less than 100.");
        }

        var (recipes, totalCount) = await recipeService.GetAllRecipesAsync(pageNumber, pageSize);

        // Include pagination metadata in the response headers 
        Response.Headers.Append("X-Total-Count", totalCount.ToString());
        Response.Headers.Append("X-Total-Pages", ((int)Math.Ceiling((double)totalCount / pageSize)).ToString());

        return Ok(recipes);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Recipe>> GetById(Guid id)
    {
        var recipe = await recipeService.GetRecipeByIdAsync(id);
        if (recipe == null)
        {
            return NotFound();
        }

        return Ok(recipe);
    }

    [HttpPost]
    public async Task<ActionResult<Recipe>> Create([FromBody] RecipeDto recipeDto)
    {
        var newRecipe = await recipeService.CreateRecipeAsync(mapper.Map<Recipe>(recipeDto));
        return CreatedAtAction(nameof(GetById), new { id = newRecipe.Id }, newRecipe);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] RecipeDto recipeDto)
    {
        var existingRecipe = await recipeService.GetRecipeByIdAsync(id);
        if (existingRecipe == null)
        {
            return NotFound();
        }

        await recipeService.UpdateRecipeAsync(id, mapper.Map(recipeDto, existingRecipe));
        return NoContent();
    }
}