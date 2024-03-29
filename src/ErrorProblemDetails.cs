﻿using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using PowerUtils.AspNetCore.ErrorHandler.Serializers;

namespace PowerUtils.AspNetCore.ErrorHandler
{
    [JsonConverter(typeof(ErrorProblemDetailsJsonConverter))]
    public class ErrorProblemDetails : ProblemDetails
    {
        /// <summary>
        /// ID generated by the problem to track logs
        /// </summary>
        [JsonPropertyName("traceId")]
        public string TraceId { get; set; }

        /// <summary>
        /// List of errors
        /// </summary>
        /// <example>
        /// { "source": { "code: "", "description": "" } }
        /// </example>
        public IDictionary<string, ErrorDetails> Errors { get; set; } = new Dictionary<string, ErrorDetails>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Initializes a new instance of <see cref="ErrorProblemDetails"/>.
        /// </summary>
        public ErrorProblemDetails() { }

        /// <summary>
        /// Serialize problem details to JSON
        /// </summary>
        public override string ToString()
            => JsonSerializer.Serialize(this);

        public static implicit operator string(ErrorProblemDetails problemDetailsResponse)
            => problemDetailsResponse.ToString();
    }


    public class ErrorDetails
    {
        /// <summary>
        /// Error code
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }


        /// <summary>
        /// A human-readable explanation specific to this occurrence of the error.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        public ErrorDetails() { }
        public ErrorDetails(
            string code,
            string description)
        {
            Code = code;
            Description = description;
        }

        public override string ToString()
            => JsonSerializer.Serialize(this);

        public static implicit operator string(ErrorDetails errorDetails)
            => errorDetails.ToString();
    }
}
