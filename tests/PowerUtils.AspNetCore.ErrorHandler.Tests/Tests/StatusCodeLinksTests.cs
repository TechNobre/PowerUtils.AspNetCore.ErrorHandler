using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Config;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class LinksAndTitlesTests
    {
        private readonly IntegrationTestsFixture _testsFixture;

        public LinksAndTitlesTests(IntegrationTestsFixture testsFixture)
            => _testsFixture = testsFixture;



        [Theory]
        [InlineData(0, "https://tools.ietf.org/html/rfc7231#section-6")]
        [InlineData(400, "https://tools.ietf.org/html/rfc9110#section-15.5.1")]
        [InlineData(401, "https://tools.ietf.org/html/rfc9110#section-15.5.2")]
        [InlineData(402, "https://tools.ietf.org/html/rfc9110#section-15.5.3")]
        [InlineData(403, "https://tools.ietf.org/html/rfc9110#section-15.5.4")]
        [InlineData(404, "https://tools.ietf.org/html/rfc9110#section-15.5.5")]
        [InlineData(405, "https://tools.ietf.org/html/rfc9110#section-15.5.6")]
        [InlineData(406, "https://tools.ietf.org/html/rfc9110#section-15.5.7")]
        [InlineData(407, "https://tools.ietf.org/html/rfc9110#section-15.5.8")]
        [InlineData(408, "https://tools.ietf.org/html/rfc9110#section-15.5.9")]
        [InlineData(409, "https://tools.ietf.org/html/rfc9110#section-15.5.10")]
        [InlineData(410, "https://tools.ietf.org/html/rfc9110#section-15.5.11")]
        [InlineData(411, "https://tools.ietf.org/html/rfc9110#section-15.5.12")]
        [InlineData(412, "https://tools.ietf.org/html/rfc9110#section-15.5.13")]
        [InlineData(413, "https://tools.ietf.org/html/rfc9110#section-15.5.14")]
        [InlineData(414, "https://tools.ietf.org/html/rfc9110#section-15.5.15")]
        [InlineData(415, "https://tools.ietf.org/html/rfc9110#section-15.5.16")]
        [InlineData(416, "https://tools.ietf.org/html/rfc9110#section-15.5.17")]
        [InlineData(417, "https://tools.ietf.org/html/rfc9110#section-15.5.18")]
        [InlineData(418, "https://tools.ietf.org/html/rfc9110#section-15.5.19")]
        [InlineData(421, "https://tools.ietf.org/html/rfc9110#section-15.5.20")]
        [InlineData(422, "https://tools.ietf.org/html/rfc9110#section-15.5.21")]
        [InlineData(424, "https://tools.ietf.org/html/rfc4918#section-11.4")]
        [InlineData(425, "https://httpwg.org/specs/rfc8470.html#status")]
        [InlineData(426, "https://tools.ietf.org/html/rfc9110#section-15.5.22")]
        [InlineData(428, "https://tools.ietf.org/html/rfc6585#section-3")]
        [InlineData(429, "https://tools.ietf.org/html/rfc6585#section-4")]
        [InlineData(431, "https://tools.ietf.org/html/rfc6585#section-5")]
        [InlineData(451, "https://httpwg.org/specs/rfc7725.html#n-451-unavailable-for-legal-reasons")]
        [InlineData(500, "https://tools.ietf.org/html/rfc9110#section-15.6.1")]
        [InlineData(501, "https://tools.ietf.org/html/rfc9110#section-15.6.2")]
        [InlineData(502, "https://tools.ietf.org/html/rfc9110#section-15.6.3")]
        [InlineData(503, "https://tools.ietf.org/html/rfc9110#section-15.6.4")]
        [InlineData(504, "https://tools.ietf.org/html/rfc9110#section-15.6.5")]
        [InlineData(505, "https://tools.ietf.org/html/rfc9110#section-15.6.6")]
        [InlineData(506, "https://tools.ietf.org/html/rfc2295#section-8.1")]
        [InlineData(507, "https://tools.ietf.org/html/rfc4918#section-11.5")]
        [InlineData(508, "https://tools.ietf.org/html/rfc5842#section-7.2")]
        [InlineData(510, "https://tools.ietf.org/html/rfc2774#section-7")]
        [InlineData(511, "https://tools.ietf.org/html/rfc6585#section-6")]
        public void StatusCodes_TryGetValue_Link(int statusCode, string link)
        {
            // Arrange
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            options.Value.ClientErrorMapping.TryGetValue(statusCode, out var act);



            // Assert
            act.Link.Should()
                .Be(link);
        }

        [Theory]
        [InlineData(0, "Unknown error")]
        [InlineData(400, "Bad Request")]
        [InlineData(401, "Unauthorized")]
        [InlineData(402, "Payment Required")]
        [InlineData(403, "Forbidden")]
        [InlineData(404, "Not Found")]
        [InlineData(405, "Method Not Allowed")]
        [InlineData(406, "Not Acceptable")]
        [InlineData(407, "Proxy Authentication Required")]
        [InlineData(408, "Request Timeout")]
        [InlineData(409, "Conflict")]
        [InlineData(410, "Gone")]
        [InlineData(411, "Length Required")]
        [InlineData(412, "Precondition Failed")]
        [InlineData(413, "Content Too Large")]
        [InlineData(414, "URI Too Long")]
        [InlineData(415, "Unsupported Media Type")]
        [InlineData(416, "Range Not Satisfiable")]
        [InlineData(417, "Expectation Failed")]
        [InlineData(418, "I'm a teapot")]
        [InlineData(421, "Misdirected Request")]
        [InlineData(422, "Unprocessable Entity")]
        [InlineData(424, "Failed Dependency")]
        [InlineData(425, "Too Early")]
        [InlineData(426, "Upgrade Required")]
        [InlineData(428, "Precondition Required")]
        [InlineData(429, "Too Many Requests")]
        [InlineData(431, "Request Header Fields Too Large")]
        [InlineData(451, "Unavailable For Legal Reasons")]
        [InlineData(500, "Internal Server Error")]
        [InlineData(501, "Not Implemented")]
        [InlineData(502, "Bad Gateway")]
        [InlineData(503, "Service Unavailable")]
        [InlineData(504, "Gateway Timeout")]
        [InlineData(505, "HTTP Version Not Supported")]
        [InlineData(506, "Variant Also Negotiates")]
        [InlineData(507, "Insufficient Storage")]
        [InlineData(508, "Loop Detected")]
        [InlineData(510, "Not Extended")]
        [InlineData(511, "Network Authentication Required")]
        public void StatusCodes_TryGetValue_Title(int statusCode, string title)
        {
            // Arrange
            var options = _testsFixture.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            options.Value.ClientErrorMapping.TryGetValue(statusCode, out var act);



            // Assert
            act.Title.Should()
                .Be(title);
        }
    }
}
