using Microsoft.AspNetCore.Mvc;
using RecipeProcessing.Api.Validation;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController(IImageService imageService, IRecipeService recipeService) : ControllerBase
{
    [HttpPost("process")]
    public async Task<IActionResult> Process(FileUpload fileUpload)
    {
        //Process the image
        var result = await imageService.Process(
            fileUpload.ImageFile!.OpenReadStream(),
            fileUpload.ImageFile.ContentType
        );
        
        await recipeService.SaveRecipeFromResult(result);

        return Ok(result);
    }
}