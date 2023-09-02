using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace PowerUtils.AspNetCore.ErrorHandler
{
    internal sealed class ApiProblemDetailsFactory : ProblemDetailsFactory, IProblemFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;
        private readonly IOptions<ErrorHandlerOptions> _errorHandlerOptions;

        public ApiProblemDetailsFactory(
            IHttpContextAccessor httpContextAccessor,
            IOptions<ApiBehaviorOptions> apiBehaviorOptions,
            IOptions<ErrorHandlerOptions> errorHandlerOptions)
        {
            _httpContextAccessor = httpContextAccessor;

            _apiBehaviorOptions = apiBehaviorOptions;
            _errorHandlerOptions = errorHandlerOptions;
        }


        public ObjectResult CreateProblemResult(
            string detail = null,
            string instance = null,
            int? statusCode = null,
            string title = null,
            string type = null,
            IDictionary<string, ErrorDetails> errors = null)
        => new ObjectResult(CreateProblem(
            detail,
            instance,
            statusCode,
            title,
            type,
            errors));

        public ErrorProblemDetails CreateProblem(
            string detail = null,
            string instance = null,
            int? statusCode = null,
            string title = null,
            string type = null,
            IDictionary<string, ErrorDetails> errors = null)
        {
            errors ??= new Dictionary<string, ErrorDetails>();

            var problemDetails = new ErrorProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance,
                Errors = errors.ToDictionary(
                    k => _errorHandlerOptions.Value.PropertyHandler(k.Key),
                    v => v.Value)
            };

            if(string.IsNullOrWhiteSpace(problemDetails.Detail))
            {
                problemDetails.Detail = errors.FirstOrDefault().Value?.Description;
            }

            _applyDefaults(_httpContextAccessor.HttpContext, problemDetails);

            return problemDetails;
        }

        internal ErrorProblemDetails Create(HttpContext httpContext)
        {
            var problemDetails = new ErrorProblemDetails();

            _applyDefaults(httpContext, problemDetails);

            return problemDetails;
        }

        public ErrorProblemDetails Create(HttpContext httpContext, IEnumerable<KeyValuePair<string, ErrorDetails>> errors)
        {
            var problemDetails = new ErrorProblemDetails();

            foreach(var error in errors)
            {
                problemDetails.Errors
                    .Add(_errorHandlerOptions.Value.PropertyHandler(error.Key),
                        error.Value);
            }

            problemDetails.Detail = errors.FirstOrDefault().Value?.Description;

            _applyDefaults(httpContext, problemDetails);

            return problemDetails;
        }

        internal ErrorProblemDetails Create(ActionContext actionContext)
        {
            var payloadTooLargeError = actionContext.ModelState.CheckPayloadTooLargeAndReturnError();
            if(payloadTooLargeError.Count == 1)
            { // When the payload is too large, we return a 413 status code
                actionContext.HttpContext.Response.StatusCode = StatusCodes.Status413RequestEntityTooLarge;

                return Create(
                   actionContext.HttpContext,
                   payloadTooLargeError);
            }


            actionContext.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            return Create(
                actionContext.HttpContext,
                actionContext.ModelState.MappingModelState());
        }

        public override ProblemDetails CreateProblemDetails(
            HttpContext httpContext,
            int? statusCode = null,
            string title = null,
            string type = null,
            string detail = null,
            string instance = null)
        {
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance,
            };

            _applyDefaults(httpContext, problemDetails);

            return problemDetails;
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(
            HttpContext httpContext,
            ModelStateDictionary modelStateDictionary,
            int? statusCode = null,
            string title = null,
            string type = null,
            string detail = null,
            string instance = null)
        {
            if(modelStateDictionary == null)
            {
                throw new ArgumentNullException(nameof(modelStateDictionary));
            }

            var problemDetails = new ValidationProblemDetails(modelStateDictionary)
            {
                Status = statusCode,
                Type = type,
                Detail = detail,
                Instance = instance,
            };

            if(title != null)
            {
                // For validation problem details, don't overwrite the default title with null.
                problemDetails.Title = title;
            }

            _applyDefaults(httpContext, problemDetails);

            return problemDetails;
        }

        private void _applyDefaults(HttpContext httpContext, ProblemDetails problemDetails)
        {
            problemDetails.Status ??= httpContext.GetStatusCode();

            if(_apiBehaviorOptions.Value.ClientErrorMapping.TryGetValue(problemDetails.Status.Value, out var clientErrorData))
            {
                problemDetails.Type ??= clientErrorData.Link;
                problemDetails.Title ??= clientErrorData.Title;
            }
            else
            {
                problemDetails.Type ??= ProblemDetailsDefaults.Defaults[0].Link;
                problemDetails.Title ??= ProblemDetailsDefaults.Defaults[0].Title;
            }

            problemDetails.Instance ??= httpContext.GetRequestEndpoint();

            problemDetails.Extensions["traceId"] = httpContext.GetCorrelationId();

            _applyDetail(problemDetails);
        }

        private static void _applyDetail(ProblemDetails problemDetails)
        {
            if(!string.IsNullOrWhiteSpace(problemDetails.Detail))
            {
                return;
            }

            problemDetails.Detail = problemDetails.Status switch
            {
                401 => "A authentication error has occurred.",
                403 => "A permissions error has occurred.",
                404 => "The entity was not found.",
                409 => "The entity already exists.",

                501 => "The feature has not been implemented.",

                _ when problemDetails.Status >= 400 && problemDetails.Status < 500 => "One or more validation errors occurred.",

                _ => "An unexpected error has occurred."
            };
        }
    }
}
