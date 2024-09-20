using Microsoft.AspNetCore.Mvc;
using RecipeProcessing.Api.Validation;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController(
    IFileService fileService,
    IRecipeService recipeService,
    IQueueService queueService,
    IHashService hashService) : ControllerBase
{
    [HttpPost("process")]
    public async Task<IActionResult> Process(FileUpload fileUpload)
    {
        var imageHash = hashService.ComputeFromStream(fileUpload.ImageFile!.OpenReadStream());
        var existingRecipe = await recipeService.GetRecipeByImageHash(imageHash);

        if (existingRecipe != null) return Ok(existingRecipe);

        // var result = await aiImageAnalysisService.Process(
        //     fileUpload.ImageFile!.OpenReadStream(),
        //     fileUpload.ImageFile.ContentType
        // );
        //
        // await recipeService.SaveRecipeFromResult(result, imageHash);
        
        var imagePath = await fileService.SaveTemporaryFileAsync(fileUpload.ImageFile.OpenReadStream(),
            fileUpload.ImageFile.ContentType);
        
        await queueService.EnqueueImageProcessingTaskAsync(
          imagePath, imageHash
        );

        return Accepted();
    }
}