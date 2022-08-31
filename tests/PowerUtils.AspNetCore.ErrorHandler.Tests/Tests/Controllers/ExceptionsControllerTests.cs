﻿using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Config;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Utils;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests.Controllers
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class ExceptionsControllerTests
    {
        private readonly IntegrationTestsFixture _testsFixture;

        public ExceptionsControllerTests(IntegrationTestsFixture testsFixture)
            => _testsFixture = testsFixture;



        [Fact]
        public async void EndpointWithGenericException_Request_500()
        {
            // Arrange
            var requestUri = "/exceptions/generic";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.InternalServerError);

            content.ValidateContent(
                HttpStatusCode.InternalServerError,
                clientErrorData,
                "GET: " + requestUri,
                "An unexpected error has occurred."
            );
        }

        [Fact]
        public async void EndpointWithNotImplementedException_Request_501()
        {
            // Arrange
            var requestUri = "/exceptions/not-implemented-exception";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.NotImplemented);

            content.ValidateContent(
                HttpStatusCode.NotImplemented,
                clientErrorData,
                "GET: " + requestUri,
                "The feature has not been implemented."
            );
        }

        [Fact]
        public async void EndpointWithAggregateException_Request_501()
        {
            // Arrange
            var requestUri = "/exceptions/aggregate-inner-not-implemented-exception";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.NotImplemented);

            content.ValidateContent(
                HttpStatusCode.NotImplemented,
                clientErrorData,
                "GET: " + requestUri,
                "The feature has not been implemented."
            );
        }

        [Fact]
        public async void EndpointWithTwoAggregateException_Request_500()
        {
            // Arrange
            var requestUri = "/exceptions/aggregate-two-inner-exception";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.InternalServerError);

            content.ValidateContent(
                HttpStatusCode.InternalServerError,
                clientErrorData,
                "GET: " + requestUri,
                "An unexpected error has occurred."
            );
        }

#if NET6_0_OR_GREATER
        // Only don't work well in testing environment for dotnet 5.0 for custom error code 404. If you call for example with postman works well
        // https://github.com/dotnet/aspnetcore/issues/31024
        [Fact]
        public async void EndpointWithNotFoundException_Request_404WithErrors()
        {
            // Arrange
            var requestUri = "/exceptions/not-found";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.NotFound);

            content.ValidateContent(
                HttpStatusCode.NotFound,
                clientErrorData,
                "GET: " + requestUri,
                "The entity does not exist",
                new Dictionary<string, ErrorDetails>()
                {
                    { "prop1", new("NOT_FOUND", "The entity does not exist") }
                }
            );
        }
#endif

        [Fact]
        public async void EndpointWithNotFoundException_Request_409WithErrors()
        {
            // Arrange
            var requestUri = "/exceptions/duplicated";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.Conflict);

            content.ValidateContent(
                HttpStatusCode.Conflict,
                clientErrorData,
                "GET: " + requestUri,
                "double",
                new Dictionary<string, ErrorDetails>()
                {
                    { "prop2", new("DUPLICATED", "double") }
                }
            );
        }

        [Fact]
        public async void CustomExceptionMapper_Request_503()
        {
            // Arrange
            var requestUri = "/exceptions/test";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.ServiceUnavailable);

            content.ValidateContent(
                HttpStatusCode.ServiceUnavailable,
                clientErrorData,
                "GET: " + requestUri,
                "An unexpected error has occurred."
            );
        }

        [Fact]
        public async void CustomExcetionWithSpecificTitleAndLink_Request_582()
        {
            // Arrange
            var requestUri = "/exceptions/custom-exception";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(582);

            content.ValidateContent(
                582,
                clientErrorData,
                "GET: " + requestUri,
                "An unexpected error has occurred."
            );

            content.Type.Should()
                .Be("CustomLink");

            content.Title.Should()
                .Be("CustomTitle");
        }

        [Fact]
        public async void EndpointWithTimeoutException_Request_504StatusCode()
        {
            // Arrange
            var requestUri = "/exceptions/timeout-exception";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.GatewayTimeout);

            content.ValidateContent(
                HttpStatusCode.GatewayTimeout,
                clientErrorData,
                "GET: " + requestUri,
                "An unexpected error has occurred."
            );
        }
    }
}
