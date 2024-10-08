﻿using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PowerUtils.AspNetCore.ErrorHandler.Samples.Exceptions;
using PowerUtils.AspNetCore.ErrorHandler.Samples.Extensions;
using PowerUtils.AspNetCore.ErrorHandler.Samples.Setups;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();

            services
                .AddAuthentication(option =>
                {
                    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                }).AddJwtBearer(options
                    => options.TokenValidationParameters = new TokenValidation());

            // Validation with basic authentication
            services
                .AddAuthentication(option =>
                {
                    option.DefaultAuthenticateScheme = BasicAuthentication.AUTHENTICATION_SCHEME;
                    option.DefaultChallengeScheme = BasicAuthentication.AUTHENTICATION_SCHEME;
                })
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthentication.AUTHENTICATION_SCHEME, null);

            services.AddErrorHandler(); // To test
            services.AddErrorHandler(options =>
            {
                options.PropertyNamingPolicy = PropertyNamingPolicy.SnakeCase;

                options.ExceptionMapper<PropertyException>(exception => 599); // Only to test the override a mapping
                options.ExceptionMapper<PropertyException>(exception => (400, exception.Property, exception.Code, exception.Message));

                options.ExceptionMapper<ModelStatesException>(exception => (
                    exception.Status,
                    exception.Errors.ToDictionary(
                        k => k.Key,
                        v => new ErrorDetails(v.Value, exception.Message))));

                options.ExceptionMapper<CustomException>(exception => 582);

                options.ExceptionMapper<TestException>(exception => StatusCodes.Status503ServiceUnavailable);
            });


            services.Configure<ApiBehaviorOptions>(options
                => options.ClientErrorMapping.Add(582, new()
                {
                    Link = "CustomLink",
                    Title = "CustomTitle"
                })
            );

            // Configurations to payload size
            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = 1_048_576; // Default value (4194304 Bytes -> 4 MB) - 1048576 = 1MB
                options.MultipartBodyLengthLimit = 1_048_576; // Default value (134217728 Bytes -> 128 MB) - 1048576 = 1MB
                options.MemoryBufferThreshold = 1_048_576; // Default value (65536 Bytes -> 128 MB) - 1048576 = 1MB
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSerilog();

            app.UseErrorHandler();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints
                => endpoints.MapControllers()); // Mapping all controller
        }
    }
}
