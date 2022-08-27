using System.Net;
using System.Threading.Tasks;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Config;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Utils;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.ControllersTests
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


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


            // Assert
            response.ValidateResponse(HttpStatusCode.Unauthorized);

            content.ValidateContent(
                HttpStatusCode.Unauthorized,
                "GET: " + requestUri
            );
        }

        [Fact]
        public async Task AuthWithError_Reques_504()
        {
            // Arrange
            var requestUri = "/auth/jwt";


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


            // Assert
            response.ValidateResponse(HttpStatusCode.GatewayTimeout);

            content.ValidateContent(
                HttpStatusCode.GatewayTimeout,
                "GET: " + requestUri
            );
        }
    }
}
