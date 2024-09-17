using Microsoft.AspNetCore.Mvc;
using RecipeProcessing.Api.Validation;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController(
    IAiImageAnalysisService aiImageAnalysisService,
    IRecipeService recipeService,
    IHashService hashService) : ControllerBase
{
    [HttpPost("process")]
    public async Task<IActionResult> Process(FileUpload fileUpload)
    {
        var imageHash = hashService.ComputeHashFromStream(fileUpload.ImageFile!.OpenReadStream());
        
        //check if unique
        //if not return

        //Process the image
        var result = await aiImageAnalysisService.Process(
            fileUpload.ImageFile!.OpenReadStream(),
            fileUpload.ImageFile.ContentType
        );
        
        await recipeService.SaveRecipeFromResult(result, imageHash);
        
        return Ok(result);
    }
}