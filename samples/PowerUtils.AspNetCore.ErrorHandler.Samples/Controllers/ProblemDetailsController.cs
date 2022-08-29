using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Controllers
{
    [ApiController]
    [Route("problem-details")]
    public class ProblemDetailsController : ControllerBase
    {
        [HttpGet("base-with-object-result")]
        public IActionResult GetProblemDetailsWithObjectResult()
        {
            var result = new ProblemDetails
            {
                Type = "some url",
                Title = "some title",
                Status = (int)HttpStatusCode.BadRequest,
            };

            return new ObjectResult(result);
        }

        [HttpGet("error-problem-details-with-object-result")]
        public IActionResult GetErrorProblemDetailsWithObjectResult()
        {
            var result = new ErrorProblemDetails
            {
                Type = "some url",
                Title = "some title",
                Status = (int)HttpStatusCode.Forbidden,
            };

            return new ObjectResult(result);
        }

        [HttpGet("base-problem")]
        public IActionResult GetProblem()
            => Problem(statusCode: 409);


#if NET6_0_OR_GREATER
        [HttpGet("result-problem")]
        public IActionResult GetResultProblem()
            => new ObjectResult(
                Results.Problem(
                    statusCode: 409,
                    extensions: new Dictionary<string, object>
                    {
                        ["Errors"] = "fake struct"
                    }
                )
            );
#endif

        [HttpGet("error-problem")]
        public IActionResult GetErrorProblem()
            => new ObjectResult(
                new ErrorProblemDetails
                {
                    Status = 409,
                    Errors = new Dictionary<string, string>()
                    {
                        ["Errors"] = "fake struct"
                    }
                }
            );
    }
}
