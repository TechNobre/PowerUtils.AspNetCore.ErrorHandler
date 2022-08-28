﻿using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Config;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Utils;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests.Controllers
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class Errors4xxControllerTests
    {
        private readonly IntegrationTestsFixture _testsFixture;

        public Errors4xxControllerTests(IntegrationTestsFixture testsFixture)
            => _testsFixture = testsFixture;



        [Fact]
        public async Task EndpointWithBadRequestError_Request_400()
        {
            // Arrange
            var requestUri = "/errors-4xx/400";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.BadRequest);

            content.ValidateContent(
                HttpStatusCode.BadRequest,
                clientErrorData,
                "GET: " + requestUri
            );
        }

        [Fact]
        public async Task EndpointWithForbiddenError_Request_403()
        {
            // Arrange
            var requestUri = "/errors-4xx/403";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.Forbidden);

            content.ValidateContent(
                HttpStatusCode.Forbidden,
                clientErrorData,
                "GET: " + requestUri
            );
        }

        [Fact]
        public async Task OverrideError_Request_403WithNewTitleAndLink()
        {
            // Arrange
            var requestUri = "/errors-4xx/403";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();
            options.Value.ClientErrorMapping[403].Link = "OverrideLink";
            options.Value.ClientErrorMapping[403].Title = "OverrideTitle";


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.Forbidden);

            content.ValidateContent(
                HttpStatusCode.Forbidden,
                clientErrorData,
                "GET: " + requestUri
            );

            content.Type.Should()
                .Be("OverrideLink");

            content.Title.Should()
                .Be("OverrideTitle");
        }

        [Fact]
        public async Task EndpointWithNotFoundError_Request_404()
        {
            // Arrange
            var requestUri = "/errors-4xx/404";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.NotFound);

            content.ValidateContent(
                HttpStatusCode.NotFound,
                clientErrorData,
                "GET: " + requestUri
            );
        }

        [Fact]
        public async Task UnextientEndpoint_Request_404()
        {
            // Arrange
            var requestUri = "/errors-4xx/un-existent";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.NotFound);

            content.ValidateContent(
                HttpStatusCode.NotFound,
                clientErrorData,
                "GET: " + requestUri
            );
        }

        [Fact]
        public async Task EndpointWithConflictError_Request_409()
        {
            // Arrange
            var requestUri = "/errors-4xx/409";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.Conflict);

            content.ValidateContent(
                HttpStatusCode.Conflict,
                clientErrorData,
                "GET: " + requestUri
            );
        }

        [Fact]
        public async Task EndpointWithUnprocessableEntityError_Request_422()
        {
            // Arrange
            var requestUri = "/errors-4xx/422";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendGetAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.UnprocessableEntity);

            content.ValidateContent(
                HttpStatusCode.UnprocessableEntity,
                clientErrorData,
                "GET: " + requestUri
            );
        }

        [Fact]
        public async Task UsePOSTVerbs_Request_405()
        {
            // Arrange
            var requestUri = "/errors-4xx/422";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            (var response, var content) = await _testsFixture.Client.SendPostAsync(requestUri);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            response.ValidateResponse(HttpStatusCode.MethodNotAllowed);

            content.ValidateContent(
                HttpStatusCode.MethodNotAllowed,
                clientErrorData,
                "POST: " + requestUri
            );
        }
    }
}
