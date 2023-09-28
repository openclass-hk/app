namespace OpenclassApp.Authentication;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

public class AuthenticationHandler : SignInAuthenticationHandler<AuthenticationOptions>
{
    public AuthenticationHandler(IOptionsMonitor<AuthenticationOptions> options,
                                          ILoggerFactory logger, UrlEncoder encoder,
                                          ISystemClock clock) : base(
        options,
        logger,
        encoder,
        clock)
    {}
    
    protected override Task HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
    {
        throw new NotImplementedException();
    }
    
    protected override Task HandleSignOutAsync(AuthenticationProperties? properties)
    {
        throw new NotImplementedException();
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        return Task.FromResult(AuthenticateResult.NoResult());
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = 401;
        return Task.CompletedTask;
    }

    protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = 403;
        return Task.CompletedTask;
    }
}
