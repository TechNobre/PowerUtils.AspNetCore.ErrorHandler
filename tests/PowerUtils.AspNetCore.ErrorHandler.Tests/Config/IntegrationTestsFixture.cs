using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.Extensions.DependencyInjection;
using PowerUtils.AspNetCore.ErrorHandler.Samples;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Config
{
    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture> { }


    public class IntegrationTestsFixture : IDisposable
    {
        public HttpClient Client;

        public readonly WebAPIFactory<Startup> Factory;

        public IntegrationTestsFixture()
        {
            Factory = new WebAPIFactory<Startup>();
            Client = _createClient();
        }

        public TService GetService<TService>()
            => Factory.Services.GetService<TService>();

        private HttpClient _createClient()
        {
            var client = Factory.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            return client;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                Client.Dispose();
                Factory.Dispose();
            }
        }
    }
}
