using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Config;

public class WebAPIFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
        => builder.UseEnvironment(Environments.Staging);
}
