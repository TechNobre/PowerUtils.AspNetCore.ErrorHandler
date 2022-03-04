using Microsoft.AspNetCore.Mvc;
using PowerUtils.AspNetCore.Authentication.BasicAuth.Attributes;
using PowerUtils.AspNetCore.Authentication.JwtBearer.Attributes;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Controllers;

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
