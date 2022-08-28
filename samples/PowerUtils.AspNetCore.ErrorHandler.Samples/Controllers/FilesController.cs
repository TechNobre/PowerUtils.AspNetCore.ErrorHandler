using Microsoft.AspNetCore.Mvc;
using PowerUtils.AspNetCore.ErrorHandler.Samples.Models;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Controllers
{
    [ApiController]
    [Route("files")]
    public class FilesController : ControllerBase
    {
        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult Upload([FromForm] FileRequest _)
            => Ok();
    }
}
