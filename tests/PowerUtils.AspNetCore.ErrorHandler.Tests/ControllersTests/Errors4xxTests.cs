using System.Net;
using System.Threading.Tasks;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Config;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Utils;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.ControllersTests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class Errors4xxTests
    {
        private readonly IntegrationTestsFixture _testsFixture;

        public Errors4xxTests(IntegrationTestsFixture testsFixture)
            => _testsFixture = testsFixture;



        [Fact]
        public async Task EndpointWithBadRequestError_Request_400()
        {
            // Arrange
            var requestUri = "/errors-4xx/400";


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


            // Assert
            response.ValidateResponse(HttpStatusCode.BadRequest);

            content.ValidateContent(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task EndpointWithForbiddenError_Request_403()
        {
            // Arrange
            var requestUri = "/errors-4xx/403";


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


            // Assert
            response.ValidateResponse(HttpStatusCode.Forbidden);

            content.ValidateContent(HttpStatusCode.Forbidden, "GET: " + requestUri);
        }

        [Fact]
        public async Task EndpointWithNotFoundError_Request_404()
        {
            // Arrange
            var requestUri = "/errors-4xx/404";


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


            // Assert
            response.ValidateResponse(HttpStatusCode.NotFound);

            content.ValidateContent(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UnextientEndpoint_Request_404()
        {
            // Arrange
            var requestUri = "/errors-4xx/un-existent";


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


            // Assert
            response.ValidateResponse(HttpStatusCode.NotFound);

            content.ValidateContent(
                HttpStatusCode.NotFound,
                "GET: " + requestUri
            );
        }

        [Fact]
        public async Task EndpointWithConflictError_Request_409()
        {
            // Arrange
            var requestUri = "/errors-4xx/409";


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


            // Assert
            response.ValidateResponse(HttpStatusCode.Conflict);

            content.ValidateContent(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task EndpointWithUnprocessableEntityError_Request_422()
        {
            // Arrange
            var requestUri = "/errors-4xx/422";


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


            // Assert
            response.ValidateResponse(HttpStatusCode.UnprocessableEntity);

            content.ValidateContent(HttpStatusCode.UnprocessableEntity);
        }

        [Fact]
        public async Task UsePOSTVerbs_Request_405()
        {
            // Arrange
            var requestUri = "/errors-4xx";


            // Act
            (var response, var content) = await _testsFixture.Client.SendPostAsync(requestUri);


            // Assert
            response.ValidateResponse(HttpStatusCode.MethodNotAllowed);

            content.ValidateContent(
                HttpStatusCode.MethodNotAllowed,
                "POST: " + requestUri
            );
        }
    }
}
