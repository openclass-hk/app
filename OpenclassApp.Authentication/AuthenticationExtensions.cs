namespace OpenclassApp.Authentication;

using Microsoft.AspNetCore.Authentication;

public static class AuthenticationExtensions
{
    public static AuthenticationBuilder AddOpenclassScheme(this AuthenticationBuilder builder)
    {
        builder.AddScheme<AuthenticationOptions, AuthenticationHandler>(
            AuthenticationConstants.AuthenticationScheme,
            _ => {});
        return builder;
    }
}
