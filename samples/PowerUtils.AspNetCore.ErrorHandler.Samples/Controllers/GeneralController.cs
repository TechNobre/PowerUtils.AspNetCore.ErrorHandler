using System;
using Microsoft.AspNetCore.Mvc;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Controllers
{
    [ApiController]
    [Route("general")]
    public class GeneralController : ControllerBase
    {
        [HttpGet]
        public string GetVersion()
            => Environment.Version.ToString();
    }
}
