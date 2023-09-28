namespace OpenclassApp.GoogleSignIn.Internal.Handlers.Initiation;

using Context;
using Google;
using Microsoft.AspNetCore.Http;
using Proofing;

internal class InitiationHandler
{
    private readonly ContextSerializer _contextSerializer;
    private readonly UriFactory _uriFactory;

    public InitiationHandler(ContextSerializer contextSerializer, UriFactory uriFactory)
    {
        _contextSerializer = contextSerializer;
        _uriFactory = uriFactory;
    }

    public async Task HandleRequestAsync(HttpContext httpContext)
    {
        var signInContext = SignInContext.Create();
        signInContext.ProofKey = ProofKey.Generate();
        var redirectUri = httpContext.GetQueryParameter("redirect_uri");
        if (redirectUri is null)
        {
            httpContext.Response.StatusCode = 400;
            await httpContext.Response.WriteAsync("Redirect URI was not provided.");
            return;
        }
        signInContext.RedirectUri = redirectUri;
        var serializedContext = _contextSerializer.Serialize(signInContext);
        var authorizationUri = _uriFactory.CreateAuthorizationUri(serializedContext);
        var responseData = new InitiationResponse
        {
            AuthorizationUri = authorizationUri,
            ProofKey = signInContext.ProofKey.Value
        };
        await httpContext.Response.WriteAsJsonAsync(responseData);
    }
}
