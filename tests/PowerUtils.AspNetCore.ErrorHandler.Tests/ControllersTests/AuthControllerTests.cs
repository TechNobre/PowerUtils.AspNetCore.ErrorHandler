using System.Net;
using System.Threading.Tasks;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Config;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Utils;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.ControllersTests;

[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class AuthControllerTests
{
    private readonly IntegrationTestsFixture _testsFixture;

    public AuthControllerTests(IntegrationTestsFixture testsFixture)
        => _testsFixture = testsFixture;


    [Fact(DisplayName = "Request an endpoint without - Should return a response with status code 401")]
    public async Task Auth_WithoutCredentials_401()
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

    [Fact(DisplayName = "Request an endpoint with an error in authentication - Should return a response with status code 504")]
    public async Task Auth_WithError_504()
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
