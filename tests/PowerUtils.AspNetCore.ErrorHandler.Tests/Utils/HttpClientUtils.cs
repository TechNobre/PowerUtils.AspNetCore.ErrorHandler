using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using PowerUtils.Text;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Utils
{
    internal static class HttpClientUtils
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static async Task<(HttpResponseMessage Response, ErrorProblemDetails Content)> SendGetAsync(this HttpClient client, string endpoint, object parameters = null)
        {
            if(parameters != null)
            {
                endpoint += parameters.ToQueryString();
            }

            var response = await client.GetAsync(endpoint);

            return (
                response,
                await response.DeserializeResponseAsync());
        }


        public static async Task<(HttpResponseMessage Response, ErrorProblemDetails Content)> SendPostMultipartAsync(this HttpClient client, string endpoint, MultipartFormDataContent body)
        {
            var response = await client
                .PostAsync(endpoint, body);

            return (
                response,
                await response.DeserializeResponseAsync());
        }

        public static async Task<(HttpResponseMessage Response, ErrorProblemDetails Content)> SendPostAsync(this HttpClient client, string endpoint, object body = null)
        {
            var request = body.ToPostRequest(endpoint);

            var response = await client
                .SendAsync(request);

            return (
                response,
                await response.DeserializeResponseAsync());
        }

        public static HttpRequestMessage ToPostRequest(this object body, string endpoint)
        {
            if(body == null)
            {
                return new HttpRequestMessage(HttpMethod.Post, endpoint)
                {
                    Content = JsonContent.Create(null, typeof(object), new MediaTypeHeaderValue(MediaTypeNames.Application.Json), null)
                };
            }

            return new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = JsonContent.Create(body)
            };
        }


        public static async Task<ErrorProblemDetails> DeserializeResponseAsync(this HttpResponseMessage responseMessage)
        {
            var content = await responseMessage.Content.ReadAsStringAsync();

            if(string.IsNullOrWhiteSpace(content))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<ErrorProblemDetails>(
                    content,
                    _jsonSerializerOptions);
            }
            catch(Exception exception)
            {
                throw new InvalidCastException($"Cannot deserialize the response to {typeof(ErrorProblemDetails).FullName} Content -> {content}", exception);
            }
        }
    }
}
