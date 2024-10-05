using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Setups
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
#if NET8_0_OR_GREATER
        public BasicAuthenticationHandler(
           IOptionsMonitor<AuthenticationSchemeOptions> options,
           ILoggerFactory logger,
           UrlEncoder encoder
        ) : base(options, logger, encoder) { }
#else
        public BasicAuthenticationHandler(
           IOptionsMonitor<AuthenticationSchemeOptions> options,
           ILoggerFactory logger,
           UrlEncoder encoder,
           ISystemClock clock
        ) : base(options, logger, encoder, clock) { }
#endif


        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                var endpoint = Context.GetEndpoint();

                var anonymousAttribute = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>();
                var authorizeAttribute = endpoint?.Metadata?.GetMetadata<AuthorizeAttribute>();

                if(anonymousAttribute != null || authorizeAttribute == null)
                { // Skip authentication if endpoint has [AllowAnonymous] attribute or does not have any authentication attribute
                    return Task.FromResult(AuthenticateResult.NoResult());
                }

                if(!Request.Headers.TryGetValue("Authorization", out var authorization))
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid internal authentication"));
                }

                var authHeader = AuthenticationHeaderValue.Parse(authorization);
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter));
                var credentialsArray = credentials.Split(':');
                var username = credentialsArray[0];
                var password = credentialsArray[1];

                if("username" != username || "password" != password)
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid internal authentication"));
                }
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid internal authentication"));
            }

            var claims = new[] {
            new Claim(ClaimTypes.Name, "username")
        };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
