using Microsoft.AspNetCore.Mvc;
using RecipeProcessing.Api.Validation;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController(
    IAiImageAnalysisService aiImageAnalysisService,
    IRecipeService recipeService,
    IQueueService queueService,
    IHashService hashService) : ControllerBase
{
    [HttpPost("process")]
    public async Task<IActionResult> Process(FileUpload fileUpload)
    {
        var imageHash = hashService.ComputeFromStream(fileUpload.ImageFile!.OpenReadStream());
        // var existingRecipe = await recipeService.GetRecipeByImageHash(imageHash);
        //
        // if (existingRecipe != null) return Ok(existingRecipe);

        // var result = await aiImageAnalysisService.Process(
        //     fileUpload.ImageFile!.OpenReadStream(),
        //     fileUpload.ImageFile.ContentType
        // );
        //
        // await recipeService.SaveRecipeFromResult(result, imageHash);
        
        await queueService.EnqueueImageProcessingTaskAsync(
            fileUpload.ImageFile!.OpenReadStream(),
            fileUpload.ImageFile.Name, imageHash
        );

        return Accepted();
    }
}