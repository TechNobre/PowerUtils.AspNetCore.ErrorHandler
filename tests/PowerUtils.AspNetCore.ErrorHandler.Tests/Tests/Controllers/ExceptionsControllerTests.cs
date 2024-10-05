using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Config;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Utils;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests.Controllers
{
    public sealed class ExceptionsControllerTests : IClassFixture<IntegrationTestsFixture>
    {
        private readonly IntegrationTestsFixture _factory;

        public ExceptionsControllerTests(IntegrationTestsFixture factory)
            => _factory = factory;



        [Fact]
        public async Task EndpointWithGenericException_Request_500()
        {
            // Arrange
            var requestUri = "/exceptions/generic";
            var options = _factory.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            using(new AssertionScope())
            {
                response.ValidateResponse(HttpStatusCode.InternalServerError);

                content.ValidateContent(
                    HttpStatusCode.InternalServerError,
                    clientErrorData,
                    "GET: " + requestUri,
                    "An unexpected error has occurred.");
            }
        }

        [Fact]
        public async Task EndpointWithNotImplementedException_Request_501()
        {
            // Arrange
            var requestUri = "/exceptions/not-implemented-exception";
            var options = _factory.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert

            using(new AssertionScope())
            {
                response.ValidateResponse(HttpStatusCode.NotImplemented);

                content.ValidateContent(
                    HttpStatusCode.NotImplemented,
                    clientErrorData,
                    "GET: " + requestUri,
                    "The feature has not been implemented.");
            }
        }

        [Fact]
        public async Task EndpointWithUnauthorizedAccessException_Request_401()
        {
            // Arrange
            var requestUri = "/exceptions/unauthorized-access-exception";
            var options = _factory.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            using(new AssertionScope())
            {
                response.ValidateResponse(HttpStatusCode.Unauthorized);

                content.ValidateContent(
                    HttpStatusCode.Unauthorized,
                    clientErrorData,
                    "GET: " + requestUri,
                    "A authentication error has occurred.");
            }
        }

        [Fact]
        public async Task EndpointWithAggregateException_Request_501()
        {
            // Arrange
            var requestUri = "/exceptions/aggregate-inner-not-implemented-exception";
            var options = _factory.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            using(new AssertionScope())
            {
                response.ValidateResponse(HttpStatusCode.NotImplemented);

                content.ValidateContent(
                    HttpStatusCode.NotImplemented,
                    clientErrorData,
                    "GET: " + requestUri,
                    "The feature has not been implemented.");
            }
        }

        [Fact]
        public async Task EndpointWithTwoAggregateException_Request_500()
        {
            // Arrange
            var requestUri = "/exceptions/aggregate-two-inner-exception";
            var options = _factory.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            using(new AssertionScope())
            {
                response.ValidateResponse(HttpStatusCode.InternalServerError);

                content.ValidateContent(
                    HttpStatusCode.InternalServerError,
                    clientErrorData,
                    "GET: " + requestUri,
                    "An unexpected error has occurred.");
            }
        }

#if NET6_0_OR_GREATER
        // Only don't work well in testing environment for dotnet 5.0 for custom error code 404. If you call for example with postman works well
        // https://github.com/dotnet/aspnetcore/issues/31024
        [Fact]
        public async Task EndpointWithNotFoundException_Request_404WithErrors()
        {
            // Arrange
            var requestUri = "/exceptions/not-found";
            var options = _factory.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            using(new AssertionScope())
            {
                response.ValidateResponse(HttpStatusCode.NotFound);

                content.ValidateContent(
                    HttpStatusCode.NotFound,
                    clientErrorData,
                    "GET: " + requestUri,
                    "The entity does not exist",
                    new Dictionary<string, ErrorDetails>()
                    {
                        ["prop1"] = new("NOT_FOUND", "The entity does not exist")
                    });
            }
        }
#endif

        [Fact]
        public async Task EndpointWithNotFoundException_Request_409WithErrors()
        {
            // Arrange
            var requestUri = "/exceptions/duplicated";
            var options = _factory.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            using(new AssertionScope())
            {
                response.ValidateResponse(HttpStatusCode.Conflict);

                content.ValidateContent(
                    HttpStatusCode.Conflict,
                    clientErrorData,
                    "GET: " + requestUri,
                    "double",
                    new Dictionary<string, ErrorDetails>()
                    {
                        ["prop2"] = new("DUPLICATED", "double")
                    });
            }
        }

        [Fact]
        public async Task CustomExceptionMapper_Request_503()
        {
            // Arrange
            var requestUri = "/exceptions/test";
            var options = _factory.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            using(new AssertionScope())
            {
                response.ValidateResponse(HttpStatusCode.ServiceUnavailable);

                content.ValidateContent(
                    HttpStatusCode.ServiceUnavailable,
                    clientErrorData,
                    "GET: " + requestUri,
                    "An unexpected error has occurred.");
            }
        }

        [Fact]
        public async Task CustomExcetionWithSpecificTitleAndLink_Request_582()
        {
            // Arrange
            var requestUri = "/exceptions/custom-exception";
            var options = _factory.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            using(new AssertionScope())
            {
                response.ValidateResponse(582);

                content.ValidateContent(
                    582,
                    clientErrorData,
                    "GET: " + requestUri,
                    "An unexpected error has occurred.");

                content.Type.Should().Be("CustomLink");
                content.Title.Should().Be("CustomTitle");
            }
        }

        [Fact]
        public async Task EndpointWithTimeoutException_Request_504StatusCode()
        {
            // Arrange
            var requestUri = "/exceptions/timeout-exception";
            var options = _factory.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            using(new AssertionScope())
            {
                response.ValidateResponse(HttpStatusCode.GatewayTimeout);

                content.ValidateContent(
                    HttpStatusCode.GatewayTimeout,
                    clientErrorData,
                    "GET: " + requestUri,
                    "An unexpected error has occurred.");
            }
        }

        [Fact]
        public async Task PropertyException_Request_400WithErrors()
        {
            // Arrange
            var requestUri = "/exceptions/property-exception";
            var options = _factory.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            using(new AssertionScope())
            {
                response.ValidateResponse(HttpStatusCode.BadRequest);

                content.ValidateContent(
                    HttpStatusCode.BadRequest,
                    clientErrorData,
                    "GET: " + requestUri,
                    "Error validations",
                    new Dictionary<string, ErrorDetails>()
                    {
                        ["prop"] = new("Err", "Error validations")
                    });
            }
        }
    }
}
