﻿using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerUtils.AspNetCore.ErrorHandler.Samples.Exceptions;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("exceptions")]
    public class ExceptionsController : ControllerBase
    {
        [HttpGet("generic")]
        public IActionResult Generic()
            => throw new Exception("Generic...");

        [HttpGet("not-implemented-exception")]
        public IActionResult NotFiniteNumberException()
            => throw new NotImplementedException("NotImplementedException...");

        [HttpGet("unauthorized-access-exception")]
        public IActionResult UnauthorizedAccessException()
            => throw new UnauthorizedAccessException();

        [HttpGet("aggregate-inner-not-implemented-exception")]
        public IActionResult AggregateExceptionInnerNotImplementedException()
            => throw new AggregateException(new NotImplementedException("AggregateException..."));

        [HttpGet("aggregate-two-inner-exception")]
        public IActionResult AggregateExceptionWithTwoInnerExceptions()
            => throw new AggregateException(
                new NotImplementedException("AggregateException..."),
                new NotImplementedException("AggregateException...")
            );

        [HttpGet("not-found")]
        public IActionResult NotFoundException()
            => throw new NotFoundException("The entity does not exist");

        [HttpGet("duplicated")]
        public IActionResult DuplicatedException()
            => throw new DuplicatedException();

        [HttpGet("test")]
        public IActionResult TestException()
            => throw new TestException();

        [HttpGet("custom-exception")]
        public IActionResult CustomException()
            => throw new CustomException();

        [HttpGet("timeout-exception")]
        public IActionResult TimeoutException()
            => throw new TimeoutException();

        [HttpGet("property-exception")]
        public IActionResult PropertyException()
            => throw new PropertyException("Error validations")
            {
                Property = "Prop",
                Code = "Err",
            };
    }
}
