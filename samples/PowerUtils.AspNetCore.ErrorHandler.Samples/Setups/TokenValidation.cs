using System;
using Microsoft.IdentityModel.Tokens;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Setups;

public class TokenValidation : TokenValidationParameters
{
    public TokenValidation()
        => throw new TimeoutException();
}
