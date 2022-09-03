using System.Collections.Generic;

namespace PowerUtils.AspNetCore.ErrorHandler
{
    internal static class ProblemDetailsDefaults
    {
        public const string PROBLEM_MEDIA_TYPE_JSON = "application/problem+json";

        internal const int FALLBACK_STATUS_CODE = 500;

        internal const string DEFAULT_ERROR_CODE = "INVALID";

        internal static readonly Dictionary<int, (string Title, string Link)> Defaults = new()
        {
            [0] = ("Unknown error", "https://tools.ietf.org/html/rfc7231#section-6"),

            // 4XX
            [400] = ("Bad Request", "https://tools.ietf.org/html/rfc9110#section-15.5.1"),
            [401] = ("Unauthorized", "https://tools.ietf.org/html/rfc9110#section-15.5.2"),
            [402] = ("Payment Required", "https://tools.ietf.org/html/rfc9110#section-15.5.3"),
            [403] = ("Forbidden", "https://tools.ietf.org/html/rfc9110#section-15.5.4"),
            [404] = ("Not Found", "https://tools.ietf.org/html/rfc9110#section-15.5.5"),
            [405] = ("Method Not Allowed", "https://tools.ietf.org/html/rfc9110#section-15.5.6"),
            [406] = ("Not Acceptable", "https://tools.ietf.org/html/rfc9110#section-15.5.7"),
            [407] = ("Proxy Authentication Required", "https://tools.ietf.org/html/rfc9110#section-15.5.8"),
            [408] = ("Request Timeout", "https://tools.ietf.org/html/rfc9110#section-15.5.9"),
            [409] = ("Conflict", "https://tools.ietf.org/html/rfc9110#section-15.5.10"),
            [410] = ("Gone", "https://tools.ietf.org/html/rfc9110#section-15.5.11"),
            [411] = ("Length Required", "https://tools.ietf.org/html/rfc9110#section-15.5.12"),
            [412] = ("Precondition Failed", "https://tools.ietf.org/html/rfc9110#section-15.5.13"),
            [413] = ("Content Too Large", "https://tools.ietf.org/html/rfc9110#section-15.5.14"),
            [414] = ("URI Too Long", "https://tools.ietf.org/html/rfc9110#section-15.5.15"),
            [415] = ("Unsupported Media Type", "https://tools.ietf.org/html/rfc9110#section-15.5.16"),
            [416] = ("Range Not Satisfiable", "https://tools.ietf.org/html/rfc9110#section-15.5.17"),
            [417] = ("Expectation Failed", "https://tools.ietf.org/html/rfc9110#section-15.5.18"),
            [418] = ("I'm a teapot", "https://tools.ietf.org/html/rfc9110#section-15.5.19"),
            [421] = ("Misdirected Request", "https://tools.ietf.org/html/rfc9110#section-15.5.20"),
            [422] = ("Unprocessable Entity", "https://tools.ietf.org/html/rfc9110#section-15.5.21"),
            [424] = ("Failed Dependency", "https://tools.ietf.org/html/rfc4918#section-11.4"),
            [425] = ("Too Early", "https://httpwg.org/specs/rfc8470.html#status"),
            [426] = ("Upgrade Required", "https://tools.ietf.org/html/rfc9110#section-15.5.22"),
            [428] = ("Precondition Required", "https://tools.ietf.org/html/rfc6585#section-3"),
            [429] = ("Too Many Requests", "https://tools.ietf.org/html/rfc6585#section-4"),
            [431] = ("Request Header Fields Too Large", "https://tools.ietf.org/html/rfc6585#section-5"),
            [451] = ("Unavailable For Legal Reasons", "https://httpwg.org/specs/rfc7725.html#n-451-unavailable-for-legal-reasons"),

            // 5XX
            [500] = ("Internal Server Error", "https://tools.ietf.org/html/rfc9110#section-15.6.1"),
            [501] = ("Not Implemented", "https://tools.ietf.org/html/rfc9110#section-15.6.2"),
            [502] = ("Bad Gateway", "https://tools.ietf.org/html/rfc9110#section-15.6.3"),
            [503] = ("Service Unavailable", "https://tools.ietf.org/html/rfc9110#section-15.6.4"),
            [504] = ("Gateway Timeout", "https://tools.ietf.org/html/rfc9110#section-15.6.5"),
            [505] = ("HTTP Version Not Supported", "https://tools.ietf.org/html/rfc9110#section-15.6.6"),
            [506] = ("Variant Also Negotiates", "https://tools.ietf.org/html/rfc2295#section-8.1"),
            [507] = ("Insufficient Storage", "https://tools.ietf.org/html/rfc4918#section-11.5"),
            [508] = ("Loop Detected", "https://tools.ietf.org/html/rfc5842#section-7.2"),
            [510] = ("Not Extended", "https://tools.ietf.org/html/rfc2774#section-7"),
            [511] = ("Network Authentication Required", "https://tools.ietf.org/html/rfc6585#section-6")
        };
    }
}
