using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.WebUtilities;
using PowerUtils.Net.Constants;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.ControllersTests;

public static class ProblemDetailsValidation
{
    public static void ValidateContent(this ProblemDetailsResponse problemDetails, HttpStatusCode statusCode)
        => problemDetails.ValidateContent(statusCode, null);

    public static void ValidateContent(this ProblemDetailsResponse problemDetails, HttpStatusCode statusCode, string instance)
    {
        var code = (int)statusCode;

        problemDetails.Status.Should()
            .Be(code);

        problemDetails.Type.Should()
            .Be(code.GetStatusCodeLink());

        problemDetails.Title.Should()
            .Be(ReasonPhrases.GetReasonPhrase(code));

        problemDetails.Instance.Should()
            .Be(instance);

        problemDetails.TraceID.Should()
            .NotBeNullOrWhiteSpace();
    }

    public static void ValidateContent(this ProblemDetailsResponse problemDetails, HttpStatusCode statusCode, string instance, Dictionary<string, string> expectedErrors)
    {
        problemDetails.ValidateContent(statusCode, instance);

        foreach(var error in expectedErrors)
        {
            problemDetails.Errors.Should()
                .Contain(error.Key, error.Value);
        }

        problemDetails.Errors.Should()
            .HaveCount(expectedErrors.Count);
    }

    public static void ValidateResponse(this HttpResponseMessage response, HttpStatusCode statusCode)
    {
        response.ValidateStatusCode(statusCode);
        response.ValidateContentType();
    }

    public static void ValidateStatusCode(this HttpResponseMessage response, HttpStatusCode statusCode)
        => response.StatusCode.Should()
            .Be(statusCode);

    public static void ValidateContentType(this HttpResponseMessage response)
        => response.Content.Headers.ContentType.MediaType.Should()
            .Be(ExtendedMediaTypeNames.ProblemApplication.JSON);
}
