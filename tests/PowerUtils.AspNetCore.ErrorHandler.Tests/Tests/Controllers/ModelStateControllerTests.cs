using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Config;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Utils;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests.Controllers
{
    public sealed class ModelStateControllerTests : IClassFixture<IntegrationTestsFixture>
    {
        private readonly IntegrationTestsFixture _factory;

        public ModelStateControllerTests(IntegrationTestsFixture factory)
            => _factory = factory;



        [Fact]
        public async Task InvalidPayload_Request_400()
        {
            // Arrange
            var requestUri = "/model-state";
            var options = _factory.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendPostAsync(requestUri, "dsfsdf");
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
        public async Task PostWithoutBody_Request_400()
        {
            // Arrange
            var requestUri = "/model-state";
            var options = _factory.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendPostAsync(requestUri, null);
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
        public async Task PostWithInvalidParameters_Request_400()
        {
            // Arrange
            var requestUri = "/model-state";
            var options = _factory.GetService<IOptions<ApiBehaviorOptions>>();
            var body = new
            {
                Description = "fake"
            };


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendPostAsync(requestUri, body);
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
        public async Task PostInvalidDeserialization_Request_400()
        {
            // Arrange
            var requestUri = "/model-state";
            var options = _factory.GetService<IOptions<ApiBehaviorOptions>>();
            var body = new
            {
                Name = "fake",
                Details = new
                {
                    Height = "dfd"
                }
            };


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendPostAsync(requestUri, body);
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
