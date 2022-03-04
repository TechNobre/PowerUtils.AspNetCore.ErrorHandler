using Microsoft.AspNetCore.Mvc;
using PowerUtils.AspNetCore.ErrorHandler.Samples.Models;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Controllers;

[ApiController]
[Route("model-state")]
public class ModelStateController : ControllerBase
{
    [HttpPost]
    public IActionResult Post(Product _)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return NoContent();
    }
}
