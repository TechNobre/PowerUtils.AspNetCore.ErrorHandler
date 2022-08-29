using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Config;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Utils;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests.Controllers
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class ProblemFactoryControllerTests
    {
        private readonly IntegrationTestsFixture _testsFixture;

        public ProblemFactoryControllerTests(IntegrationTestsFixture testsFixture)
            => _testsFixture = testsFixture;



        [Fact]
        public async Task ObjectResult_CreateProblemResult_403()
        {
            // Arrange
            var requestUri = "/problem-factory/create-result";


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


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

            content.TraceId.Should()
                .NotBeNullOrWhiteSpace();

            content.Errors.Should()
                .HaveCount(2);

            content.Errors.Should()
                    .Contain("Key4", "Error4");
            content.Errors.Should()
                    .Contain("Key14", "Error124");
        }

        [Fact]
        public async Task Problem_CreateProblem_429()
        {
            // Arrange
            var requestUri = "/problem-factory/create-problem";


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


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

            content.TraceId.Should()
                .NotBeNullOrWhiteSpace();

            content.Errors.Should()
                .HaveCount(3);

            content.Errors.Should()
                    .Contain("Key100", "Error114");
            content.Errors.Should()
                    .Contain("Key114", "Error11124");
            content.Errors.Should()
                    .Contain("me", "ti");
        }
    }
}
