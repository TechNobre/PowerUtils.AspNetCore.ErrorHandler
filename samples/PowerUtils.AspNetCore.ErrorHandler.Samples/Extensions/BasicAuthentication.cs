using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Extensions
{
    public class BasicAuthentication : AuthorizeAttribute
    {
        public const string AUTHENTICATION_SCHEME = "Basic";

        public BasicAuthentication()
            => AuthenticationSchemes = AUTHENTICATION_SCHEME;
    }
}
