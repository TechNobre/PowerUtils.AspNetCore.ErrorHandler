using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PowerUtils.AspNetCore.ErrorHandler.Samples;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Config
{
    public sealed class IntegrationTestsFixture : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
            => builder.UseEnvironment(Environments.Staging);

        protected override void ConfigureClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        }

        public TService GetService<TService>()
            => Services.GetService<TService>();
    }
}
