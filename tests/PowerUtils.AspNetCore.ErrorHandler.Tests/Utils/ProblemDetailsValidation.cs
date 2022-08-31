using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Utils
{
    public static class ProblemDetailsValidation
    {
        public static void ValidateContent(this ErrorProblemDetails problemDetails, HttpStatusCode statusCode, ClientErrorData clientErrorData, string instance, string detail)
            => problemDetails.ValidateContent(
                (int)statusCode,
                clientErrorData,
                instance,
                detail
            );

        public static void ValidateContent(this ErrorProblemDetails problemDetails, int statusCode, ClientErrorData clientErrorData, string instance, string detail)
        {
            problemDetails.Status.Should()
                .Be(statusCode);

            problemDetails.Type.Should()
                .Be(clientErrorData.Link);

            problemDetails.Title.Should()
                .Be(clientErrorData.Title);

            problemDetails.Instance.Should()
                .Be(instance);

            problemDetails.Detail.Should()
                .Be(detail);

            problemDetails.TraceId.Should()
                .NotBeNullOrWhiteSpace();
        }

        public static void ValidateContent(this ErrorProblemDetails problemDetails, HttpStatusCode statusCode, ClientErrorData clientErrorData, string instance, string detail, Dictionary<string, ErrorDetails> expectedErrors)
        {
            problemDetails.ValidateContent(statusCode, clientErrorData, instance, detail);

            problemDetails.ValidateContent(expectedErrors);
        }

        public static void ValidateContent(this ErrorProblemDetails problemDetails, Dictionary<string, ErrorDetails> expectedErrors)
        {
            foreach(var error in problemDetails.Errors)
            {
                expectedErrors[error.Key].Code.Should()
                    .Be(error.Value.Code);

                expectedErrors[error.Key].Description.Should()
                    .Be(error.Value.Description);
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
