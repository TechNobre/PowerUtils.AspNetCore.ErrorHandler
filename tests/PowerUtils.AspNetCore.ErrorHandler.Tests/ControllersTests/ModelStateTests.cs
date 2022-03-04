using System.Net;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Config;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Utils;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.ControllersTests;

[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class ModelStateTests
{
    private readonly IntegrationTestsFixture _testsFixture;

    public ModelStateTests(IntegrationTestsFixture testsFixture)
        => _testsFixture = testsFixture;


    [Fact(DisplayName = "The request with body invalid - Should return a response with status code 400 and error code RequestBody:INVALID")]
    public async void Post_Invalid_400()
    {
        // Arrange
        var requestUri = "/model-state";


        // Act
        (var response, var content) = await _testsFixture.Client.SendPostAsync(requestUri, "dsfsdf");


        // Assert
        response.ValidateResponse(HttpStatusCode.BadRequest);

        content.ValidateContent(
            HttpStatusCode.BadRequest,
            "POST: " + requestUri,
            new()
            {
                { "request_body", "INVALID" }
            }
        );
    }

    [Fact(DisplayName = "The request without body - Should return a response with status code 400 and error code RequestBody:REQUIRED")]
    public async void Post_WithoutBody_400()
    {
        // Arrange
        var requestUri = "/model-state";


        // Act
        (var response, var content) = await _testsFixture.Client.SendPostAsync(requestUri, null);


        // Assert
        response.ValidateResponse(HttpStatusCode.BadRequest);

        content.ValidateContent(
            HttpStatusCode.BadRequest,
            "POST: " + requestUri,
            new()
            {
                { "request_body", "REQUIRED" }
            }
        );
    }

    [Fact(DisplayName = "The request with body invalid properties - Should return a response with status code 400 and errors")]
    public async void Post_InvalidParameters_400()
    {
        // Arrange
        var requestUri = "/model-state";
        var body = new
        {
            Description = "fake"
        };


        // Act
        (var response, var content) = await _testsFixture.Client.SendPostAsync(requestUri, body);


        // Assert
        response.ValidateResponse(HttpStatusCode.BadRequest);

        content.ValidateContent(
            HttpStatusCode.BadRequest,
            "POST: " + requestUri,
            new()
            {
                { "name", "The Name field is required." },
                { "description", "The field Description must be a string or array type with a maximum length of '2'." }
            }
        );
    }

    [Fact(DisplayName = "The request with invalid paramter - Should return a response with status code 400 and errors")]
    public async void Post_InvalidDeserialization_400()
    {
        // Arrange
        var requestUri = "/model-state";
        var body = new
        {
            Name = "fake",
            Details = new
            {
                Height = "dfd"
            }
        };


        // Act
        (var response, var content) = await _testsFixture.Client.SendPostAsync(requestUri, body);


        // Assert
        response.ValidateResponse(HttpStatusCode.BadRequest);

        content.ValidateContent(
            HttpStatusCode.BadRequest,
            "POST: " + requestUri,
            new()
            {
                { "details.height", "INVALID" }
            }
        );
    }
}
