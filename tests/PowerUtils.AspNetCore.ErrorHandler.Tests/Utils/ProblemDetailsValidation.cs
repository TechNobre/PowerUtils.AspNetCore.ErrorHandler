using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Utils
{
    public static class ProblemDetailsValidation
    {
        public static void ValidateContent(this ProblemDetailsResponse problemDetails, HttpStatusCode statusCode, ClientErrorData clientErrorData, string instance)
            => problemDetails.ValidateContent(
                (int)statusCode,
                clientErrorData,
                instance
            );

        public static void ValidateContent(this ProblemDetailsResponse problemDetails, int statusCode, ClientErrorData clientErrorData, string instance)
        {
            problemDetails.Status.Should()
                .Be(statusCode);

            problemDetails.Type.Should()
                .Be(clientErrorData.Link);

            problemDetails.Title.Should()
                .Be(clientErrorData.Title);

            problemDetails.Instance.Should()
                .Be(instance);

            problemDetails.TraceId.Should()
                .NotBeNullOrWhiteSpace();
        }

        public static void ValidateContent(this ProblemDetailsResponse problemDetails, HttpStatusCode statusCode, ClientErrorData clientErrorData, string instance, Dictionary<string, string> expectedErrors)
        {
            problemDetails.ValidateContent(statusCode, clientErrorData, instance);

            foreach(var error in expectedErrors)
            {
                problemDetails.Errors.Should()
                    .Contain(error.Key, error.Value);
            }

            problemDetails.Errors.Should()
                .HaveCount(expectedErrors.Count);
        }

        public static void ValidateResponse(this HttpResponseMessage response, int statusCode)
        {
            response.ValidateStatusCode(statusCode);
            response.ValidateContentType();
        }

        public static void ValidateResponse(this HttpResponseMessage response, HttpStatusCode statusCode)
        {
            response.ValidateStatusCode(statusCode);
            response.ValidateContentType();
        }

        public static void ValidateStatusCode(this HttpResponseMessage response, HttpStatusCode statusCode)
            => response.StatusCode.Should()
                .Be(statusCode);

        public static void ValidateStatusCode(this HttpResponseMessage response, int statusCode)
            => ((int)response.StatusCode).Should()
                .Be(statusCode);

        public static void ValidateContentType(this HttpResponseMessage response)
            => response.Content.Headers.ContentType.MediaType.Should()
                .Be(ProblemDetailsDefaults.PROBLEM_MEDIA_TYPE_JSON);
    }
}
