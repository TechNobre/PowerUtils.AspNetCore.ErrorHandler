using System.Collections.Generic;
using System.Net;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Config;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Utils;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.ControllersTests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class ModelStateTests
    {
        private readonly IntegrationTestsFixture _testsFixture;

        public ModelStateTests(IntegrationTestsFixture testsFixture)
            => _testsFixture = testsFixture;



        [Fact]
        public async void PostInvalid_Request_400()
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
                new Dictionary<string, string>()
                {
                    { "request_body", "INVALID" }
                }
            );
        }

        [Fact]
        public async void PostWithoutBody_Request_400()
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
                new Dictionary<string, string>()
                {
                    { "request_body", "REQUIRED" }
                }
            );
        }

        [Fact]
        public async void PostWithInvalidParameters_Request_400()
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
                new Dictionary<string, string>()
                {
                    { "name", "The Name field is required." },
                    { "description", "The field Description must be a string or array type with a maximum length of '2'." }
                }
            );
        }

        [Fact]
        public async void PostInvalidDeserialization_Request_400()
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
                new Dictionary<string, string>()
                {
                    { "details.height", "INVALID" }
                }
            );
        }
    }
}
