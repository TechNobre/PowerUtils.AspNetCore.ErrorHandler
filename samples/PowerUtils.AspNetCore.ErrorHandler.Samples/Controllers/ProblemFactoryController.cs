using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Controllers
{
    [ApiController]
    [Route("problem-factory")]
    public class ProblemFactoryController : ControllerBase
    {
        private readonly IProblemFactory _problemFactory;
        public ProblemFactoryController(IProblemFactory problemFactory)
            => _problemFactory = problemFactory;



        [HttpGet("create-result")]
        public IActionResult CreateProblemResult()
            => _problemFactory.CreateProblemResult(
                detail: "some detail",
                instance: "some instance",
                statusCode: (int)HttpStatusCode.Forbidden,
                title: "some title",
                type: "some type",
                errors: new Dictionary<string, string>
                {
                    ["Key4"] = "Error4",
                    ["Key14"] = "Error124",
                }
            );

        [HttpGet("create-problem")]
        public IActionResult CreateProblem()
            => new ObjectResult(_problemFactory.CreateProblem(
                detail: "fake detail",
                instance: "fake instance",
                statusCode: (int)HttpStatusCode.TooManyRequests,
                title: "fake title",
                type: "fake type",
                errors: new Dictionary<string, string>
                {
                    ["Key100"] = "Error114",
                    ["Key114"] = "Error11124",
                    ["me"] = "ti"
                }
            ));
    }
}
