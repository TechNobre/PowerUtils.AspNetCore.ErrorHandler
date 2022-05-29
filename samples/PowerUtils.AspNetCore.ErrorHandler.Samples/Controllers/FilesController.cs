using Microsoft.AspNetCore.Mvc;
using PowerUtils.AspNetCore.ErrorHandler.Samples.Models;
using PowerUtils.Net.Constants;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Controllers;

[ApiController]
[Route("files")]
public class FilesController : ControllerBase
{
    [HttpPost]
    [Consumes(ExtendedMediaTypeNames.Multipart.FORM_DATA)]
    public IActionResult Upload([FromForm] FileRequest _)
        => Ok();
}
