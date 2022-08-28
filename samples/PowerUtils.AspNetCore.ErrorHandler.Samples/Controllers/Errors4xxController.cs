using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("errors-4xx")]
    public class Errors4xxController : ControllerBase
    {
        [HttpGet("400")]
        public IActionResult Error400()
            => BadRequest();

        [HttpGet("403")]
        public IActionResult Error403()
            => Forbid();

        [HttpGet("404")]
        public IActionResult Error404()
            => NotFound();

        [HttpGet("409")]
        public IActionResult Error409()
            => Conflict();

        [HttpGet("422")]
        public IActionResult Error422()
            => UnprocessableEntity();
    }
}
