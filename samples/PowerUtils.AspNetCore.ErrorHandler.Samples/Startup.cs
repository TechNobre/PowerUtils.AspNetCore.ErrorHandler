using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PowerUtils.AspNetCore.Authentication.BasicAuth.Attributes;
using PowerUtils.AspNetCore.ErrorHandler.Samples.Exceptions;
using PowerUtils.AspNetCore.ErrorHandler.Samples.Setups;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples;

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
                => options.TokenValidationParameters = new TokenValidation()
            );

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

            options.ExceptionMapper<ModelStatesException>(exception => 599); // Only to test the override a mapping
            options.ExceptionMapper<ModelStatesException>(exception => (exception.Status, exception.Errors));

            options.ExceptionMapper<TestException>(exception => StatusCodes.Status503ServiceUnavailable);
            options.ExceptionMapper<TimeoutException>(exception => StatusCodes.Status504GatewayTimeout);
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
            => endpoints.MapControllers() // Mapping all controller
        );
    }
}
