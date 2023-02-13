using System;
using System.Collections.Generic;
using System.Globalization;
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
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class FilesControllerTests
    {
        private readonly IntegrationTestsFixture _testsFixture;

        public FilesControllerTests(IntegrationTestsFixture testsFixture)
            => _testsFixture = testsFixture;



        [Fact]
        public async Task FileLarge_Send_413()
        {
            // Arrange
            var requestUri = "/files";
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();

            var file = FileUtils.LoadFile("../../../../../media/test_3_23mb.jpg");
            var body = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
            body.Add(file, "FileFake", "upload.jpg");


            // Act
            await _testsFixture.Client.SendPostMultipartAsync(requestUri, body);
            (var response, var content) = await _testsFixture.Client.SendPostMultipartAsync(requestUri, body);
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
    }
}
