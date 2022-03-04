using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PowerUtils.Security;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Setups;
public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(
       IOptionsMonitor<AuthenticationSchemeOptions> options,
       ILoggerFactory logger,
       UrlEncoder encoder,
       ISystemClock clock
    ) : base(options, logger, encoder, clock) { }

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

            if(!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid internal authentication"));
            }

            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            (var username, var password) = authHeader.Parameter.FromBasicAuth();

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
