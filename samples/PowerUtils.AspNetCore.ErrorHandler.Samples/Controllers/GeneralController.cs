using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Controllers
{
    [ApiController]
    [Route("general")]
    public class GeneralController : ControllerBase
    {
        private readonly IOptions<ApiBehaviorOptions> _options;
        public GeneralController(IOptions<ApiBehaviorOptions> options)
            => _options = options;


        [HttpGet("version")]
        public string GetVersion()
            => Environment.Version.ToString();

        [HttpGet("type-and-title/{statusCode:int}")]
        public IActionResult GetTypeAndTitle(int statusCode)
        {
            string link;
            string title;
            if(_options.Value.ClientErrorMapping.TryGetValue(statusCode, out var errorData))
            {
                link = errorData.Link;
                title = errorData.Title;
            }
            else
            {
                link = "fake link";
                title = "fake title";
            }

            return Ok(new
            {
                link,
                title
            });
        }
    }
}
