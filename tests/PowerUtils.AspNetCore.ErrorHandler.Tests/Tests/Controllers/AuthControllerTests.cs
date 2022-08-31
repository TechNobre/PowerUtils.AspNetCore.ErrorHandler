using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Config;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Utils;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests.Controllers
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class AuthControllerTests
    {
        private readonly IntegrationTestsFixture _testsFixture;

        public AuthControllerTests(IntegrationTestsFixture testsFixture)
            => _testsFixture = testsFixture;



        [Fact]
        public async Task AuthWithoutCredentials_Reques_401()
        {
            // Arrange
            var requestUri = "/auth/basic";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.Unauthorized);

            content.ValidateContent(
                HttpStatusCode.Unauthorized,
                clientErrorData,
                "GET: " + requestUri,
                "A authentication error has occurred."
            );
        }

        [Fact]
        public async Task AuthWithError_Reques_504()
        {
            // Arrange
            var requestUri = "/auth/jwt";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.GatewayTimeout);

            content.ValidateContent(
                HttpStatusCode.GatewayTimeout,
                clientErrorData,
                "GET: " + requestUri,
                "An unexpected error has occurred."
            );
        }
    }
}
