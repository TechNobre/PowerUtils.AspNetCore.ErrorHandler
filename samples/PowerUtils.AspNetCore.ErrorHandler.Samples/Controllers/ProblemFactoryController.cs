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
                errors: new Dictionary<string, ErrorDetails>()
                {
                    ["Key4"] = new("Error4", "description 111"),
                    ["Key14"] = new("Error124", "description 423423")
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
                errors: new Dictionary<string, ErrorDetails>()
                {
                    ["Key100"] = new("Error114", "description fake"),
                    ["Key114"] = new("Error11124", "description 1444"),
                    ["me"] = new("ti", "111"),
                    ["MyKey"] = new("MyCode", "MyDisc")
                }
            ));

        [HttpGet("null-errors")]
        public IActionResult NullErrors()
            => new ObjectResult(_problemFactory.CreateProblem(
                detail: "fake detail",
                instance: "fake instance",
                statusCode: (int)HttpStatusCode.BadRequest,
                title: "fake title",
                type: "fake type",
                errors: null
            ));
    }
}
