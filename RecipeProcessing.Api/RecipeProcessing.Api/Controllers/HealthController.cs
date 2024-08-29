using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RecipeProcessing.Api.Controllers;

public class HealthController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("Pong - RecipeProcess.Api is up and running");
    }
}