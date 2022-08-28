using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace PowerUtils.AspNetCore.ErrorHandler
{
    public static class HttpContextExtensions
    {
        private static readonly HashSet<string> _allowedHeaderNames = new(StringComparer.OrdinalIgnoreCase)
        {
            HeaderNames.AccessControlAllowCredentials,
            HeaderNames.AccessControlAllowHeaders,
            HeaderNames.AccessControlAllowMethods,
            HeaderNames.AccessControlAllowOrigin,
            HeaderNames.AccessControlExposeHeaders,
            HeaderNames.AccessControlMaxAge,

            HeaderNames.StrictTransportSecurity,

            HeaderNames.WWWAuthenticate,
        };


        public static string GetCorrelationId(this HttpContext httpContext)
        {
            var result = Activity.Current?.Id;
            if(!string.IsNullOrWhiteSpace(result))
            {
                return result;
            }

            result = httpContext?.TraceIdentifier;
            if(string.IsNullOrWhiteSpace(result))
            {
                result = $"guid:{Guid.NewGuid()}";
            }

            return result;
        }


        internal static string GetRequestEndpoint(this HttpContext httpContext)
        {
            if(httpContext?.Request == null)
            {
                return null;
            }

            return httpContext.Request.Method + ": " + httpContext.Request.Path.Value;
        }

        /*
         * Informational responses ( 100 – 199 )
         * Successful responses    ( 200 – 299 )
         * Redirection messages    ( 300 – 399 )
         * Client error responses  ( 400 – 499 )
         * Server error responses  ( 500 – 599 )
        */
        internal static int? GetStatusCode(this HttpContext httpContext)
            => httpContext?.Response?.StatusCode;

        internal static bool IsNotSuccess(this HttpContext httpContext)
        {
            var statusCode = httpContext.GetStatusCode();
            if(statusCode >= 400)
            {
                return true;
            }

            if(statusCode is null)
            {
                return true;
            }

            return false;
        }

        internal static void ResetResponse(this HttpContext httpContext)
        {
            // Make sure problem responses are never cached.
            var headers = new HeaderDictionary();
            headers.Append(HeaderNames.CacheControl, "no-cache, no-store, must-revalidate");
            headers.Append(HeaderNames.Pragma, "no-cache");
            headers.Append(HeaderNames.Expires, "0");

            if(httpContext.Response != null)
            {
                // Because the CORS middleware adds all the headers early in the pipeline,
                // we want to copy over the existing Access-Control-* headers after resetting the response.
                var headersContained = httpContext
                    .Response
                    .Headers
                    .Where(w => _allowedHeaderNames.Contains(w.Key));


                foreach(var header in headersContained)
                {
                    headers.Add(header);
                }

                httpContext.Response.Clear();

                // ProblemDetails has it's own content type
                httpContext.Response.ContentType = ProblemDetailsDefaults.PROBLEM_MEDIA_TYPE_JSON;

                foreach(var header in headers)
                {
                    httpContext.Response.Headers.Add(header);
                }
            }
        }

        internal static void ResetResponse(this HttpContext httpContext, int statusCode)
        {
            httpContext.ResetResponse();
            httpContext.Response.StatusCode = statusCode;
        }

        internal static async Task WriteProblemDetailsResponseAsync(this HttpContext httpContext, ProblemDetailsResponse response)
        {
            httpContext.ResetResponse();

            httpContext.Response.StatusCode = response.Status; // It is required because was a `ResetResponse()`

            // Serialize the problem details object to the Response as JSON (using System.Text.Json)
            await JsonSerializer.SerializeAsync(httpContext.Response.Body, response);
        }
    }
}
