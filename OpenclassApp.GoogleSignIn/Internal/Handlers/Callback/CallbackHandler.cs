namespace OpenclassApp.GoogleSignIn.Internal.Handlers.Callback;

using Context;
using Google;
using Microsoft.AspNetCore.Http;

internal class CallbackHandler
{
    private readonly ContextSerializer _contextSerializer;
    private readonly UriFactory _uriFactory;

    public CallbackHandler(ContextSerializer contextSerializer, UriFactory uriFactory)
    {
        _contextSerializer = contextSerializer;
        _uriFactory = uriFactory;
    }

    public async Task HandleRequestAsync(HttpContext httpContext)
    {
        var state = httpContext.GetQueryParameter("state");
        var authorizationCode = httpContext.GetQueryParameter("code");
        if (state is null || authorizationCode is null)
        {
            httpContext.Response.StatusCode = 400;
            await httpContext.Response.WriteAsync("Bad request.");
            return;
        }
        var signInContext = _contextSerializer.Deserialize(state);
        if (signInContext is null)
        {
            httpContext.Response.StatusCode = 400;
            await httpContext.Response.WriteAsync("Bad request.");
            return;
        }

        // TODO: Backchannel exchange

        signInContext.SignedInUserId = "1234";
        // We'll use the serialized context as an authorization code that, with the proof key, can
        // be used to complete a successful sign in.
        var serializedContext = _contextSerializer.Serialize(signInContext);
        var redirectUri = signInContext.RedirectUri + "?code=" + serializedContext;
        httpContext.Response.Redirect(redirectUri);
    }
}
