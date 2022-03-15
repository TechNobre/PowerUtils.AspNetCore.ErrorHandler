using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PowerUtils.AspNetCore.ErrorHandler.Handlers;

namespace PowerUtils.AspNetCore.ErrorHandler
{
    public static class ErrorHandlerExtensions
    {
        /// <summary>
        /// Adds the required services for <see cref="UseErrorHandler"/> to work correctly,
        /// using the default options.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="options">Options for handling exceptions and errors</param>
        public static IServiceCollection AddErrorHandler(this IServiceCollection services, Action<ErrorHandlerOptions> options = null)
        {
            if(options == null)
            {
                services.AddOptions<ErrorHandlerOptions>();
            }
            else
            {
                services.Configure(options);
            }

            services.AddScoped<ProblemDetailsFactory>();
            services.AddModelStateMiddleware();

            return services;
        }

        /// <summary>
        /// Adds the <see cref="ErrorHandlerMiddleware"/> and <see cref="ExceptionHandlerMiddleware"/> to the application pipeline.
        /// </summary>
        /// <param name="app">The application builder to add the middleware to.</param>
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app)
        {
            ExceptionHandlerMiddleware.UseExceptionHandlerMiddleware(app);

            app.UseMiddleware<ErrorHandlerMiddleware>();

            return app;
        }
    }
}
