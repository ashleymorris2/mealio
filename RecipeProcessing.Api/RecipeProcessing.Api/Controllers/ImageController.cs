using Microsoft.AspNetCore.Mvc;
using RecipeProcessing.Core.Interfaces;

namespace RecipeProcessing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController(IImageService imageService) : ControllerBase
{
    [HttpPost("process")]
    public IActionResult Process(IFormFile? imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            return BadRequest("No image uploaded.");
        }
        
        //Process the image
        var result = imageService.Process(imageFile.OpenReadStream());
        
        return Ok("Image processed successfully.");
    }
}