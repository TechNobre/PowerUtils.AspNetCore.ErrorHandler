using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using PowerUtils.AspNetCore.ErrorHandler.Samples;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Config
{
    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture> { }


    public class IntegrationTestsFixture : IDisposable
    {
        public HttpClient Client;

        private readonly WebAPIFactory<Startup> _factory;

        public IntegrationTestsFixture()
        {
            _factory = new WebAPIFactory<Startup>();
            Client = _createClient();
        }

        private HttpClient _createClient()
        {
            var client = _factory.CreateClient();
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
                _factory.Dispose();
            }
        }
    }
}
