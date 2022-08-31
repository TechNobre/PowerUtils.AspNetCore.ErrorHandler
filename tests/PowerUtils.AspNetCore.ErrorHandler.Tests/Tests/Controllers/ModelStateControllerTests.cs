using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Config;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Utils;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests.Controllers
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class ModelStateControllerTests
    {
        private readonly IntegrationTestsFixture _testsFixture;

        public ModelStateControllerTests(IntegrationTestsFixture testsFixture)
            => _testsFixture = testsFixture;



        [Fact]
        public async void InvalidPayload_Request_400()
        {
            // Arrange
            var requestUri = "/model-state";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendPostAsync(requestUri, "dsfsdf");
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.BadRequest);

            content.ValidateContent(
                HttpStatusCode.BadRequest,
                clientErrorData,
                "POST: " + requestUri,
                "The payload is invalid.",
                new Dictionary<string, ErrorDetails>()
                {
                    { "payload", new("INVALID", "The payload is invalid.") }
                }
            );
        }

        [Fact]
        public async void PostWithoutBody_Request_400()
        {
            // Arrange
            var requestUri = "/model-state";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendPostAsync(requestUri, null);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.BadRequest);

            content.ValidateContent(
                HttpStatusCode.BadRequest,
                clientErrorData,
                "POST: " + requestUri,
                "The payload is required.",
                new Dictionary<string, ErrorDetails>()
                {
                    { "payload", new("REQUIRED", "The payload is required.") }
    }
            );
        }

        [Fact]
        public async void PostWithInvalidParameters_Request_400()
        {
            // Arrange
            var requestUri = "/model-state";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();
            var body = new
            {
                Description = "fake"
            };


            // Act
            (var response, var content) = await _testsFixture.Client.SendPostAsync(requestUri, body);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.BadRequest);

            content.ValidateContent(
                HttpStatusCode.BadRequest,
                clientErrorData,
                "POST: " + requestUri,
                "The Name field is required.",
                new Dictionary<string, ErrorDetails>()
                {
                    { "name", new("INVALID", "The Name field is required.") },
                    { "description", new("INVALID", "The field Description must be a string or array type with a maximum length of '2'.") }
                }
            );
        }

        [Fact]
        public async void PostInvalidDeserialization_Request_400()
        {
            // Arrange
            var requestUri = "/model-state";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();
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
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.BadRequest);

            content.ValidateContent(
                HttpStatusCode.BadRequest,
                clientErrorData,
                "POST: " + requestUri,
                "The payload is invalid.",
                new Dictionary<string, ErrorDetails>()
                {
                    { "details.height", new("INVALID", "The payload is invalid.") }
                }
            );
        }
    }
}
