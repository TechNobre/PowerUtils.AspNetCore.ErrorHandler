using Microsoft.AspNetCore.Mvc;
using PowerUtils.AspNetCore.ErrorHandler.Samples.Extensions;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        [BasicAuthentication]
        [HttpGet("basic")]
        public IActionResult BasicAuth()
            => Ok();

        [JWTAuthentication]
        [HttpGet("jwt")]
        public IActionResult JWTAuth()
            => Ok();
    }
}
