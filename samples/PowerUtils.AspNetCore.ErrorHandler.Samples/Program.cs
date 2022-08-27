using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PowerUtils.AspNetCore.ErrorHandler.Samples.Setups;
using Serilog;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                _createBootstrapLogger();

                Log.Information($"[STARTING] {Assembly.GetEntryAssembly()?.GetName()?.Name}");


                CreateHostBuilder(args)
                    .Build()
                    .Run();


                return 0;
            }
            catch(Exception exception)
            {
                Log.Fatal(exception, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration)
                    => context.Configure(configuration, services)
                )
                .ConfigureWebHostDefaults(webBuilder
                    => webBuilder.UseStartup<Startup>()
                );

        private static void _createBootstrapLogger()
        {
            var configuration = _getConfiguration();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateBootstrapLogger();
        }

        private static IConfigurationRoot _getConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", true)
                .Build();

            return configuration;
        }
    }
}
