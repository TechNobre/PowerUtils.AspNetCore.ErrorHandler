using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Config;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Utils;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.ControllersTests;

[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class FilesControllerTests
{
    private readonly IntegrationTestsFixture _testsFixture;

    public FilesControllerTests(IntegrationTestsFixture testsFixture)
        => _testsFixture = testsFixture;

    [Fact(DisplayName = "Request an endpoint with large payload - Should return a response with status code 413")]
    public async Task File_Large_413()
    {
        // Arrange
        var requestUri = "/files";

        var file = FileUtils.LoadFile("../../../media/test_3_23mb.jpg");
        var body = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
        body.Add(file, "FileFake", "upload.jpg");


        // Act
        (var response, var content) = await _testsFixture.Client.SendPostMultipartAsync(requestUri, body);


        // Assert
        response.ValidateResponse(HttpStatusCode.RequestEntityTooLarge);

        content.ValidateContent(
            HttpStatusCode.RequestEntityTooLarge,
            "POST: " + requestUri,
            new()
            {
                { "request_body", "MAX:1048576" }
            }
        );
    }
}
