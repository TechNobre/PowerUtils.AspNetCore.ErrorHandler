using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Config;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Utils;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests.Controllers
{
    public sealed class ProblemFactoryControllerTests : IClassFixture<IntegrationTestsFixture>
    {
        private readonly IntegrationTestsFixture _factory;

        public ProblemFactoryControllerTests(IntegrationTestsFixture factory)
            => _factory = factory;



        [Fact]
        public async Task ObjectResult_CreateProblemResult_403()
        {
            // Arrange
            var requestUri = "/problem-factory/create-result";


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendGetAsync(requestUri);


            // Assert
            response.ValidateResponse(HttpStatusCode.Forbidden);

            content.Status.Should()
                .Be((int)HttpStatusCode.Forbidden);

            content.Type.Should()
                .Be("some type");

            content.Title.Should()
                .Be("some title");

            content.Instance.Should()
                .Be("some instance");

            content.Detail.Should()
               .Be("some detail");

            content.TraceId.Should()
                .NotBeNullOrWhiteSpace();

            content.ValidateContent(new Dictionary<string, ErrorDetails>()
            {
                ["key4"] = new("Error4", "description 111"),
                ["key14"] = new("Error124", "description 423423")
            });
        }

        [Fact]
        public async Task Problem_CreateProblem_429()
        {
            // Arrange
            var requestUri = "/problem-factory/create-problem";


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendGetAsync(requestUri);


            // Assert
            response.ValidateResponse(HttpStatusCode.TooManyRequests);

            content.Status.Should()
                .Be((int)HttpStatusCode.TooManyRequests);

            content.Type.Should()
                .Be("fake type");

            content.Title.Should()
                .Be("fake title");

            content.Instance.Should()
                .Be("fake instance");

            content.Detail.Should()
               .Be("fake detail");

            content.TraceId.Should()
                .NotBeNullOrWhiteSpace();

            content.ValidateContent(new Dictionary<string, ErrorDetails>()
            {
                ["key100"] = new("Error114", "description fake"),
                ["key114"] = new("Error11124", "description 1444"),
                ["me"] = new("ti", "111"),
                ["my_key"] = new("MyCode", "MyDisc")
            });
        }

        [Fact]
        public async Task WithoutErrors_CreateProblem_400()
        {
            // Arrange
            var requestUri = "/problem-factory/null-errors";


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendGetAsync(requestUri);


            // Assert
            response.ValidateResponse(HttpStatusCode.BadRequest);

            content.Status.Should()
                .Be((int)HttpStatusCode.BadRequest);

            content.Type.Should()
                .Be("fake type");

            content.Title.Should()
                .Be("fake title");

            content.Instance.Should()
                .Be("fake instance");

            content.Detail.Should()
               .Be("fake detail");

            content.TraceId.Should()
                .NotBeNullOrWhiteSpace();

            content.ValidateContent(new Dictionary<string, ErrorDetails>());
        }
    }
}
