using System.Collections.Generic;
using System.Net;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Config;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Utils;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.ControllersTests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class Exceptions5xxTests
    {
        private readonly IntegrationTestsFixture _testsFixture;

        public Exceptions5xxTests(IntegrationTestsFixture testsFixture)
            => _testsFixture = testsFixture;



        [Fact]
        public async void EndpointWithGenericException_Request_500()
        {
            // Arrange
            var requestUri = "/exceptions/generic";


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


            // Assert
            response.ValidateResponse(HttpStatusCode.InternalServerError);

            content.ValidateContent(HttpStatusCode.InternalServerError, "GET: " + requestUri);
        }

        [Fact]
        public async void EndpointWithNotImplementedException_Request_501()
        {
            // Arrange
            var requestUri = "/exceptions/not-implemented-exception";


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


            // Assert
            response.ValidateResponse(HttpStatusCode.NotImplemented);

            content.ValidateContent(HttpStatusCode.NotImplemented, "GET: " + requestUri);
        }

        [Fact]
        public async void EndpointWithAggregateException_Request_501()
        {
            // Arrange
            var requestUri = "/exceptions/aggregate-inner-not-implemented-exception";


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


            // Assert
            response.ValidateResponse(HttpStatusCode.NotImplemented);

            content.ValidateContent(HttpStatusCode.NotImplemented, "GET: " + requestUri);
        }

        [Fact]
        public async void EndpointWithTwoAggregateException_Request_500()
        {
            // Arrange
            var requestUri = "/exceptions/aggregate-two-inner-exception";


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);


            // Assert
            response.ValidateResponse(HttpStatusCode.InternalServerError);

            content.ValidateContent(HttpStatusCode.InternalServerError, "GET: " + requestUri);
        }

#if NET6_0_OR_GREATER
        // Only don't work well in testing environment for dotnet 5.0 for custom error code 404. If you call for example with postman works well
        // https://github.com/dotnet/aspnetcore/issues/31024
        [Fact]
        public async void EndpointWithNotFoundException_Request_404WithErrors()
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
                new Dictionary<string, string>()
                {
                    { "prop1", "NOT_FOUND" }
                }
            );
        }
#endif

        [Fact]
        public async void EndpointWithNotFoundException_Request_409WithErrors()
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
                new Dictionary<string, string>()
                {
                    { "prop2", "DUPLICATED" }
                }
            );
        }

        [Fact]
        public async void CustomExceptionMapper_Request_503()
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
}
