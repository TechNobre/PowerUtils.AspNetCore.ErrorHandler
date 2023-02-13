using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace PowerUtils.AspNetCore.ErrorHandler
{
    public interface IProblemFactory
    {
        /// <summary>
        /// Produces a <see cref="ObjectResult"/> response.
        /// </summary>
        /// <param name="statusCode">The value for <see cref="ProblemDetails.Status" />.</param>
        /// <param name="detail">The value for <see cref="ProblemDetails.Detail" />.</param>
        /// <param name="instance">The value for <see cref="ProblemDetails.Instance" />.</param>
        /// <param name="title">The value for <see cref="ProblemDetails.Title" />.</param>
        /// <param name="type">The value for <see cref="ProblemDetails.Type" />.</param>
        /// <param name="errors">The value for <see cref="ErrorProblemDetails.Errors" />.</param>
        /// <returns>The created <see cref="ObjectResult"/> for the response.</returns>
        ObjectResult CreateProblemResult(
            string detail = null,
            string instance = null,
            int? statusCode = null,
            string title = null,
            string type = null,
            IDictionary<string, ErrorDetails> errors = null);

        /// <summary>
        /// Produces a <see cref="ErrorProblemDetails"/> response.
        /// </summary>
        /// <param name="statusCode">The value for <see cref="ProblemDetails.Status" />.</param>
        /// <param name="detail">The value for <see cref="ProblemDetails.Detail" />.</param>
        /// <param name="instance">The value for <see cref="ProblemDetails.Instance" />.</param>
        /// <param name="title">The value for <see cref="ProblemDetails.Title" />.</param>
        /// <param name="type">The value for <see cref="ProblemDetails.Type" />.</param>
        /// <param name="errors">The value for <see cref="ErrorProblemDetails.Errors" />.</param>
        /// <returns>The created <see cref="ErrorProblemDetails"/> for the response.</returns>
        ErrorProblemDetails CreateProblem(
            string detail = null,
            string instance = null,
            int? statusCode = null,
            string title = null,
            string type = null,
            IDictionary<string, ErrorDetails> errors = null);
    }
}
