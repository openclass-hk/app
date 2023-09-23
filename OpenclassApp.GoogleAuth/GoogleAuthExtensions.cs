namespace OpenclassApp.GoogleAuth;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

public static class GoogleAuthExtensions
{
    public static AuthenticationBuilder AddGoogleAuth(this IServiceCollection builder,
                                                      Action<GoogleAuthOptions>? configureOptions)
    {
        var gauthOptions = new GoogleAuthOptions
        {
            ClientId = string.Empty,
            ClientSecret = string.Empty
        };
        configureOptions?.Invoke(gauthOptions);
        return builder
               .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddOAuth(GoogleAuthConstants.AuthenticationScheme,
                         options =>
                         {
                             options.ClientId = gauthOptions.ClientId;
                             options.ClientSecret = gauthOptions.ClientSecret;

                             options.AuthorizationEndpoint =
                                 "https://accounts.google.com/o/oauth2/v2/auth";
                             options.TokenEndpoint = "https://oauth2.googleapis.com/token";
                             options.UserInformationEndpoint =
                                 "https://www.googleapis.com/oauth2/v3/userinfo ";

                             options.Scope.Add("openid");
                             options.CallbackPath = "/oauth/callback";
                             options.SignInScheme =
                                 CookieAuthenticationDefaults.AuthenticationScheme;

                             options.Events.OnCreatingTicket = HandleCreatingTicketEvent;
                         });
    }

    private static Task HandleCreatingTicketEvent(OAuthCreatingTicketContext context)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var rootElement = context.TokenResponse.Response!.RootElement;
        var idTokenJwt = rootElement.GetString("id_token")!;
        var idToken = jwtHandler.ReadJwtToken(idTokenJwt);
        context.Principal!.Identities.First().AddClaims(idToken.Claims);
        return Task.CompletedTask;
    }
}
