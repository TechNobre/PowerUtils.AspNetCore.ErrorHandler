using System.Net;
using System.Threading.Tasks;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Config;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Utils;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.ControllersTests;

[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class Errors4xxTests
{
    private readonly IntegrationTestsFixture _testsFixture;

    public Errors4xxTests(IntegrationTestsFixture testsFixture)
        => _testsFixture = testsFixture;


    [Fact(DisplayName = "The request should return a problem details response with status code 400")]
    public async Task Error_400()
    {
        // Arrange
        var requestUri = "/errors-4xx/400";


        // Act
        (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


        // Assert
        response.ValidateResponse(HttpStatusCode.BadRequest);

        content.ValidateContent(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "The request should return a problem details response with status code 403")]
    public async Task Error_403()
    {
        // Arrange
        var requestUri = "/errors-4xx/403";


        // Act
        (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


        // Assert
        response.ValidateResponse(HttpStatusCode.Forbidden);

        content.ValidateContent(HttpStatusCode.Forbidden, "GET: " + requestUri);
    }

    [Fact(DisplayName = "The request should return a problem details response with status code 404")]
    public async Task Error_404()
    {
        // Arrange
        var requestUri = "/errors-4xx/404";


        // Act
        (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


        // Assert
        response.ValidateResponse(HttpStatusCode.NotFound);

        content.ValidateContent(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Call an un-existent endpoint - Should return problem details response with status code 404")]
    public async Task Enpoint_UnExistent_404()
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

    [Fact(DisplayName = "The request should return a problem details response with error code 409")]
    public async Task Error_409()
    {
        // Arrange
        var requestUri = "/errors-4xx/409";


        // Act
        (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


        // Assert
        response.ValidateResponse(HttpStatusCode.Conflict);

        content.ValidateContent(HttpStatusCode.Conflict);
    }

    [Fact(DisplayName = "The request should return a problem details response with error code 422")]
    public async Task Error_422()
    {
        // Arrange
        var requestUri = "/errors-4xx/422";


        // Act
        (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


        // Assert
        response.ValidateResponse(HttpStatusCode.UnprocessableEntity);

        content.ValidateContent(HttpStatusCode.UnprocessableEntity);
    }

    [Fact(DisplayName = "Using the verb POST to call an endpoint with verb GET - Should return a problem details response with error code 405")]
    public async Task GetEndpoint_UsePOSTVerbs_405()
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
