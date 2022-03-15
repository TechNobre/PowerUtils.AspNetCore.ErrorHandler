using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PowerUtils.AspNetCore.ErrorHandler.Samples.Setups;
using Serilog;
using Serilog.Events;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples;

public static class Program
{
    public static int Main(string[] args)
    {
        try
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger();

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
}
