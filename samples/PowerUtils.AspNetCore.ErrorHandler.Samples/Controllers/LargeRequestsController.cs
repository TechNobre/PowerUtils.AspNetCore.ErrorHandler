using Microsoft.AspNetCore.Mvc;
using PowerUtils.AspNetCore.ErrorHandler.Samples.Models;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Controllers
{
    [ApiController]
    [Route("large-requests")]
    public class LargeRequestsController : ControllerBase
    {
        [HttpPost("file")]
        [Consumes("multipart/form-data")]
        public IActionResult Upload([FromForm] FileRequest _)
            => Ok();

        [HttpPost("text")]
        public IActionResult Text(string _)
            => Ok();
    }
}
