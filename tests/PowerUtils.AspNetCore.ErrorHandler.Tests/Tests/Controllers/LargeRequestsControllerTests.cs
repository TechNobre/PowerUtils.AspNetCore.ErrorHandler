using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Config;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Utils;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests.Controllers
{
    public sealed class LargeRequestsControllerTests : IClassFixture<IntegrationTestsFixture>
    {
        private readonly IntegrationTestsFixture _factory;

        public LargeRequestsControllerTests(IntegrationTestsFixture factory)
            => _factory = factory;



        [Fact]
        public async Task LargeFile_Send_413()
        {
            // Arrange
            var requestUri = "/large-requests/file";
            var options = _factory.GetService<IOptions<ApiBehaviorOptions>>();

            var filePath = Path.GetFullPath("../../../../../media/test_3_23mb.jpg");
            var file = new StreamContent(new MemoryStream(File.ReadAllBytes(filePath)));
            var body = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
            body.Add(file, "FileFake", "upload.jpg");


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendPostMultipartAsync(requestUri, body);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            using(new AssertionScope())
            {
                response.ValidateResponse(HttpStatusCode.RequestEntityTooLarge);

                content.ValidateContent(
                    HttpStatusCode.RequestEntityTooLarge,
                    clientErrorData,
                    "POST: " + requestUri,
                    "The payload is too big.",
                    new Dictionary<string, ErrorDetails>()
                    {
                        { "payload", new("MAX:1048576", "The payload is too big.") }
                    });
            }
        }

        [Fact]
        public async Task FakeLargeFile_Send_413()
        {
            // Arrange
            var requestUri = "/large-requests/file";
            var options = _factory.GetService<IOptions<ApiBehaviorOptions>>();

            var file = new StreamContent(new MemoryStream(new byte[1_048_577]));
            var body = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
            body.Add(file, "FileFake", "upload.jpg");


            // Act
            (var response, var content) = await _factory
                .CreateClient()
                .SendPostMultipartAsync(requestUri, body);
            options.Value.ClientErrorMapping.TryGetValue((int)response.StatusCode, out var clientErrorData);


            // Assert
            using(new AssertionScope())
            {
                response.ValidateResponse(HttpStatusCode.RequestEntityTooLarge);

                content.ValidateContent(
                    HttpStatusCode.RequestEntityTooLarge,
                    clientErrorData,
                    "POST: " + requestUri,
                    "The payload is too big.",
                    new Dictionary<string, ErrorDetails>()
                    {
                        { "payload", new("MAX:1048576", "The payload is too big.") }
                    });
            }
        }

        [Fact]
        public async Task FakeFileWithLimitSize_Send_200()
        {
            // Arrange
            var requestUri = "/large-requests/file";

            var file = new StreamContent(new MemoryStream(new byte[1_048_576]));
            var body = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
            body.Add(file, "FileFake", "upload.jpg");


            // Act
            (var response, _) = await _factory
                .CreateClient()
                .SendPostMultipartAsync(requestUri, body);


            // Assert
            response.ValidateStatusCode(HttpStatusCode.OK);
        }
    }
}
