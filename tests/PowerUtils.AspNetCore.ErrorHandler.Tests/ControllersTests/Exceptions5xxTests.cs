using System.Net;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Config;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Utils;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.ControllersTests;

[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class Exceptions5xxTests
{
    private readonly IntegrationTestsFixture _testsFixture;

    public Exceptions5xxTests(IntegrationTestsFixture testsFixture)
        => _testsFixture = testsFixture;


    [Fact(DisplayName = "The request should return a problem details response with status code 500")]
    public async void Exception_500()
    {
        // Arrange
        var requestUri = "/exceptions/generic";


        // Act
        (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


        // Assert
        response.ValidateResponse(HttpStatusCode.InternalServerError);

        content.ValidateContent(HttpStatusCode.InternalServerError, "GET: " + requestUri);
    }

    [Fact(DisplayName = "The request should return a problem details response with status code 501")]
    public async void NotImplementedException_501()
    {
        // Arrange
        var requestUri = "/exceptions/not-implemented-exception";


        // Act
        (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


        // Assert
        response.ValidateResponse(HttpStatusCode.NotImplemented);

        content.ValidateContent(HttpStatusCode.NotImplemented, "GET: " + requestUri);
    }

    [Fact(DisplayName = "The request should return a problem details response with status code 501")]
    public async void AggregateException_WithInnerNotImplementedException_501()
    {
        // Arrange
        var requestUri = "/exceptions/aggregate-inner-not-implemented-exception";


        // Act
        (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


        // Assert
        response.ValidateResponse(HttpStatusCode.NotImplemented);

        content.ValidateContent(HttpStatusCode.NotImplemented, "GET: " + requestUri);
    }

    [Fact(DisplayName = "The request should return a problem details response with status code 500")]
    public async void AggregateException_WithTwoInnerExceptions_500()
    {
        // Arrange
        var requestUri = "/exceptions/aggregate-two-inner-exception";


        // Act
        (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


        // Assert
        response.ValidateResponse(HttpStatusCode.InternalServerError);

        content.ValidateContent(HttpStatusCode.InternalServerError, "GET: " + requestUri);
    }

    [Fact(DisplayName = "The request should return a problem details response with status code 404 with errors")]
    public async void NotFoundException_404WithErrors()
    {
        // Arrange
        var requestUri = "/exceptions/not-found";


        // Act
        (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


        // Assert
        response.ValidateResponse(HttpStatusCode.NotFound);

        content.ValidateContent(
            HttpStatusCode.NotFound,
            "GET: " + requestUri,
            new()
            {
                { "prop1", "NOT_FOUND" }
            }
        );
    }

    [Fact(DisplayName = "The request should return a problem details response with status code 409 with errors")]
    public async void NotFoundException_409WithErrors()
    {
        // Arrange
        var requestUri = "/exceptions/duplicated";


        // Act
        (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


        // Assert
        response.ValidateResponse(HttpStatusCode.Conflict);

        content.ValidateContent(
            HttpStatusCode.Conflict,
            "GET: " + requestUri,
            new()
            {
                { "prop2", "DUPLICATED" }
            }
        );
    }

    [Fact(DisplayName = "The request should return a problem details response with status code 510 with errors")]
    public async void TestException_510()
    {
        // Arrange
        var requestUri = "/exceptions/test";


        // Act
        (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


        // Assert
        response.ValidateResponse(HttpStatusCode.ServiceUnavailable);

        content.ValidateContent(
            HttpStatusCode.ServiceUnavailable,
            "GET: " + requestUri
        );
    }
}
